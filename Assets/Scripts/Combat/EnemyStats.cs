using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [HideInInspector] public SO_Enemy scriptableEnemy;
    int maxHealth;
    int currentHealth;
    float timeAlive;
    float craftingSpeed;
    
    public SO_EnemyPhase currentPhase = null;
    public SO_EnemyCombo currentCombo = null;
    public bool lastShotFired = true;
    public int currentMove;


    SpriteRenderer spriteRenderer;
    List<SO_EnemyPhase> enemyPhases;
    public void Setup()
    {
        currentHealth = maxHealth;
        timeAlive = 0;
        GetComponent<SpriteRenderer>().sprite = scriptableEnemy.sprite; //add sprite
        maxHealth = scriptableEnemy.baseHealth;                         //add health
        craftingSpeed = scriptableEnemy.craftingSpeed;                  //add crafting speed

        enemyPhases = scriptableEnemy.enemyPhases;                      //fill phase list
        
        //get first phase
        foreach (SO_EnemyPhase phase in scriptableEnemy.enemyPhases)
        {
            if (phase.isStartingPhase)
            {
                currentPhase = phase;
                break;
            }
        }
        //if no first phase is assigned, assigned first index
        currentPhase = scriptableEnemy.enemyPhases[0];
        
        //fire first combo
        currentPhase.StartCombo(ref currentCombo, ref lastShotFired);



        gameObject.name = scriptableEnemy.enemyName;
    }

    private void Update()
    {
        //keep track in which phase enemy should be
        timeAlive += Time.deltaTime;
        scriptableEnemy.GoToNextPhase(ref lastShotFired, ref currentCombo, ref currentPhase, currentHealth, timeAlive);

        //keep track when enemy needs to start new combo (when last shot of current combo has been fired
        if (lastShotFired)
        {
            currentPhase.StartCombo(ref currentCombo, ref lastShotFired);
        }
    }


}
