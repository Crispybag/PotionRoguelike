using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [Header("Scriptable Objects")]//build observer
    [SerializeField] private SO_PlayerStats playerStats;
    [SerializeField] private SO_PlayerMoveManager playerMoveManager;
    [SerializeField] private SO_EnemyMoveTriggerManager moveManager;
    [SerializeField] private SO_OnPlayerSteppedOnHazard hazardManager;
    [SerializeField] private SO_OnPlayerMoved playerMovementManager;



    [Header("Public Variables")]//build observer
    [SerializeField] GameObject _fireTilePrefab;


    public int _currentHealth; 
    public int _currentShield; 
    public int _maxHealth;

    public List<SO_Move.Debuff> currentDebuffs = new List<SO_Move.Debuff>();
    public List<float> currentDebuffTimes = new List<float>();
    private void Start()
    {
        _currentHealth = _maxHealth;
        playerStats.onPlayerStatsChanged(this);
        playerStats.location = transform.position;
    }
    private void OnEnable()
    {
        moveManager.onMoveReachedEnd.AddListener(RegisterHit);
        playerMoveManager.onMoveReachedEnd.AddListener(usePotion);
        hazardManager.onPlayerSteppedOnHazard.AddListener(onHazardStepped);
        playerMovementManager.onPlayerMoved.AddListener(onPlayerMoved);
    }

    private void OnDisable()
    {
        moveManager.onMoveReachedEnd.RemoveListener(RegisterHit);
        playerMoveManager.onMoveReachedEnd.RemoveListener(usePotion);
        hazardManager.onPlayerSteppedOnHazard.RemoveListener(onHazardStepped);
        playerMovementManager.onPlayerMoved.RemoveListener(onPlayerMoved);
    }

    private void usePotion(PotionStats potion)
    {
        if (potion.potionEffect == SO_Potion.PotionEffect.HEAL)
        {
            _currentHealth += potion.strength;
            if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
        }

        if (potion.potionEffect == SO_Potion.PotionEffect.SHIELD)
        {
            _currentShield += potion.strength;
        }
        playerStats.onPlayerStatsChanged(this);
    }


    private void handleBuff(SO_Move.Debuff debuff, float debuffTime)
    {
        bool isAlreadyInList = false;

        for(int i = 0; i<currentDebuffs.Count; i++)
        {
            if (currentDebuffs[i] == debuff)
            {
                currentDebuffTimes[i] += debuffTime;
                isAlreadyInList = true;
                break;
            }
        }

        if (!isAlreadyInList)
        {
            currentDebuffs.Add(debuff);
            currentDebuffTimes.Add(debuffTime);
            onDebuffStart(debuff);
        }


    }

    private void onDebuffStart(SO_Move.Debuff debuff)
    {
        switch (debuff)
        {
            case SO_Move.Debuff.BURNED:
                GridManager.mapManager.SpawnGridObject(_fireTilePrefab);
                break;        
        }

    }

    private void RegisterHit(MoveStats move)
    {
        if (move.debuffs.Count != move.debuffDurations.Count)
        {
            Debug.LogError("debuff List and Debuff duration list are not of equal size, something went wrong");
            return;
        }

        if (move.debuffs.Count > 0)
        {
            for (int i = 0; i <move.debuffs.Count; i++)
            {
                handleBuff(move.debuffs[i], move.debuffDurations[i]);
            }
        }


        if(_currentShield > 0 && move.damage > 0) 
        { 
            _currentShield--;
            playerStats.onPlayerStatsChanged(this); 
            return; 
        }
        _currentHealth -= move.damage;
        playerStats.onPlayerStatsChanged(this);
    }

    private void onHazardStepped(GridHazard hazard)
    {
        hazard.affectPlayer(ref _currentHealth);
        playerStats.onPlayerStatsChanged(this);
    }

    private void onPlayerMoved(PlayerMovement pMovement)
    {
        
    }

    private void updateTimers()
    {
        for (int i = 0; i < currentDebuffTimes.Count; i++)
        {
            currentDebuffTimes[i] -= Time.deltaTime;
        }
    }

    private void updateDebuffs()
    {
        for (int i = 0; i < currentDebuffTimes.Count; i++)
        {
            if (currentDebuffTimes[i] <= 0f)
            {
                currentDebuffTimes.RemoveAt(i);
                currentDebuffs.RemoveAt(i);
                playerStats.onPlayerStatsChanged(this);
                break;
            }
        }
    }

    private void Update()
    {
        updateTimers();
        updateDebuffs();
    }
}
