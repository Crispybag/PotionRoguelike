using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMove : MonoBehaviour
{
    [SerializeField] private GameObject baseMovePrefab;
    float timeChanneling;
    EnemyStats enemyStats;
    GameObject instantiatedMove;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //get current combo

        timeChanneling += Time.deltaTime;

        

        //if channel time is longer than the move cast time divided by how fast the enemy crafts
        if (enemyStats.currentMove >= enemyStats.currentCombo.moves.Count) return;

        if (timeChanneling / enemyStats.scriptableEnemy.craftingSpeed <= enemyStats.currentCombo.moves[enemyStats.currentMove].castTime) return;
        Debug.Log("Do Move! " + enemyStats.currentCombo.moves[enemyStats.currentMove].name);
        enemyStats.currentCombo.RegisterShot(ref enemyStats.currentMove, ref enemyStats.lastShotFired);

        timeChanneling = 0;
        instantiatedMove = Instantiate(baseMovePrefab);
        Setup(enemyStats.currentCombo.moves[enemyStats.currentMove]);

    }

    void Setup(SO_Move pMove)
    {
        instantiatedMove.GetComponent<MoveStats>().Setup(pMove, enemyStats.moveManager);
    }

}
