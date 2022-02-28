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
    [SerializeField] private SO_OnGridManagerChanged SOgridManager;
    

    [Header("Public Variables")]//build observer
    [SerializeField] GameObject _fireTilePrefab;
    [SerializeField] GameObject _poisonTilePrefab;

    public int currentHealth; 
    public int currentShield; 
    public int maxHealth;

    public List<SO_Move.Debuff> currentDebuffs = new List<SO_Move.Debuff>();
    public List<float> currentDebuffTimes = new List<float>();

    private GridManager gridManager;
    private SO_Move.Debuff _currentlyHandledDebuff = SO_Move.Debuff.NONE;


    private void Start()
    {
        currentHealth = maxHealth;
        playerStats.onPlayerStatsChanged(this);
        playerStats.location = transform.position;
    }
    private void OnEnable()
    {
        moveManager.onMoveReachedEnd.AddListener(RegisterHit);
        playerMoveManager.onMoveReachedEnd.AddListener(usePotion);
        hazardManager.onPlayerSteppedOnHazard.AddListener(onHazardStepped);
        playerMovementManager.onPlayerMoved.AddListener(onPlayerMoved);
        SOgridManager.onGridManagerChanged.AddListener(onGridManagerChanged);
    }

    private void OnDisable()
    {
        moveManager.onMoveReachedEnd.RemoveListener(RegisterHit);
        playerMoveManager.onMoveReachedEnd.RemoveListener(usePotion);
        hazardManager.onPlayerSteppedOnHazard.RemoveListener(onHazardStepped);
        playerMovementManager.onPlayerMoved.RemoveListener(onPlayerMoved);
        SOgridManager.onGridManagerChanged.AddListener(onGridManagerChanged);
    }

    private void usePotion(PotionStats potion)
    {
        if (potion.potionEffect == SO_Potion.PotionEffect.HEAL)
        {
            currentHealth += potion.strength;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }

        if (potion.potionEffect == SO_Potion.PotionEffect.SHIELD)
        {
            currentShield += potion.strength;
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
            _currentlyHandledDebuff = debuff;
            //request a gridmanager call
            if(SOgridManager.OnRequestGridManager()) handleDebuffs(debuff);
        }


    }


    private void onPlayerMoved(PlayerMovement movement)
    {
        foreach(SO_Move.Debuff debuff in currentDebuffs)
        {
            if (debuff == SO_Move.Debuff.POISONED)
            {
                Instantiate(_poisonTilePrefab, movement.startPosition, transform.rotation);
            }
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

        if(currentShield > 0 && move.damage > 0) 
        { 
            currentShield--;
            playerStats.onPlayerStatsChanged(this); 
            return; 
        }
        currentHealth -= move.damage;
        playerStats.onPlayerStatsChanged(this);
    }

    private void onHazardStepped(GridHazard hazard)
    {
        hazard.affectPlayer(ref currentHealth);
        playerStats.onPlayerStatsChanged(this);
    }



    void handleDebuffs(SO_Move.Debuff debuff)
    {
        switch (_currentlyHandledDebuff)
        {
            case SO_Move.Debuff.BURNED:
                gridManager.SpawnGridObject(_fireTilePrefab);
                break;
            case SO_Move.Debuff.POISONED:
                //Instantiate(_poisonTilePrefab, new Vector3(-900, -900, 0), transform.rotation);
                break;

            case SO_Move.Debuff.FROZEN:
                break;


            default:
                break;
        }
    }



    private void onGridManagerChanged(GridManager manager)
    {
        gridManager = manager;
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
