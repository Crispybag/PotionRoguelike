using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMove : MonoBehaviour
{
    [SerializeField] private GameObject baseMovePrefab;
    [SerializeField] private SO_PlayerStats playerStats;
    float cooldownTimer;
    float channelTimer;
    bool isChanneling = false;
    EnemyStats enemyStats;
    GameObject instantiatedMove;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //Increase the time it is channeling
        cooldownTimer += Time.deltaTime;

        //check if we're not already out of the current combo
        if (enemyStats.currentMove >= enemyStats.currentCombo.moves.Count) return;

        //if channel time is longer than the move cast time divided by how fast the enemy crafts
        if (cooldownTimer / enemyStats.scriptableEnemy.craftingSpeed <= enemyStats.currentCombo.moves[enemyStats.currentMove].castTime) return;
        
        //start channeling once cooldown is off
        channelTimer += Time.deltaTime;
        
        //do channeling stuff once
        if (!isChanneling)
        {
            isChanneling = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }

        if (channelTimer <= enemyStats.currentCombo.moves[enemyStats.currentMove].channelTime) return;


        Debug.Log("Do Move! " + enemyStats.currentCombo.moves[enemyStats.currentMove].moveName);
        enemyStats.currentCombo.RegisterShot(ref enemyStats.currentMove, ref enemyStats.lastShotFired);

        //refresh timers
        cooldownTimer = 0;
        channelTimer = 0;
        isChanneling = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        //instantiate the move
        instantiatedMove = Instantiate(baseMovePrefab, gameObject.transform);
        Setup(enemyStats.currentCombo.moves[enemyStats.currentMove]);

    }

    void Setup(SO_Move pMove)
    {
        instantiatedMove.GetComponent<MoveStats>().Setup(pMove, enemyStats.moveManager, playerStats);
    }

}
