using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [HideInInspector] public SO_Enemy scriptableEnemy;
    int maxHealth;
    public int currentHealth;
    public int currentShield;
    float timeAlive;
    float craftingSpeed;

    public SO_EnemyMoveTriggerManager moveManager;
    public SO_PlayerMoveManager playerManager;
    public SO_OnEnemyStatUpdate enemyStatManager;
    [HideInInspector] public SO_EnemyPhase currentPhase = null;
    [HideInInspector] public SO_EnemyCombo currentCombo = null;
    [HideInInspector] public bool lastShotFired = true;
    [HideInInspector] public int currentMove;


    SpriteRenderer spriteRenderer;
    List<SO_EnemyPhase> enemyPhases;
    public void Setup()
    {
        timeAlive = 0;
        GetComponent<SpriteRenderer>().sprite = scriptableEnemy.sprite; //add sprite
        maxHealth = scriptableEnemy.baseHealth;                         //add health
        currentHealth = maxHealth;
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
    private void Start()
    {
        enemyStatManager.onEnemyStatsChanged(this);
    }
    private void Update()
    {

        if (currentHealth <= 0 )
        {
            Destroy(gameObject);
        }

        //keep track in which phase enemy should be
        timeAlive += Time.deltaTime;
        scriptableEnemy.GoToNextPhase(ref lastShotFired, ref currentCombo, ref currentPhase, currentHealth, timeAlive);

        //keep track when enemy needs to start new combo (when last shot of current combo has been fired
        if (lastShotFired)
        {
            currentPhase.StartCombo(ref currentCombo, ref lastShotFired);
        }
    }

    private void OnEnable()
    {
        moveManager.onMoveReachedEnd.AddListener(doMoveEffects);
        playerManager.onMoveReachedEnd.AddListener(doPlayerMoveEffects);
    }
    private void OnDisable()
    {
        moveManager.onMoveReachedEnd.RemoveListener(doMoveEffects);
        playerManager.onMoveReachedEnd.RemoveListener(doPlayerMoveEffects);

    }

    private void doPlayerMoveEffects(PotionStats potion)
    {
        if (potion.potionEffect == SO_Potion.PotionEffect.DMG)
        {
            if(currentShield > 0)  currentShield--;
            else currentHealth -= potion.strength;

            enemyStatManager.onEnemyStatsChanged(this);
        }
    }
    private void doMoveEffects(MoveStats s)
    {
        currentHealth += s.healing;
        currentShield += s.shielding;
        enemyStatManager.onEnemyStatsChanged(this);
    }



}
