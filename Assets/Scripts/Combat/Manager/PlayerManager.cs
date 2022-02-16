using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [Header("Scriptable Objects")]//build observer
    [SerializeField] private SO_PlayerStats playerStats;
    [SerializeField] private SO_PlayerMoveManager playerMoveManager;
    [SerializeField] private SO_EnemyMoveTriggerManager moveManager;

    [Header("Public Variables")]//build observer
    public int _currentHealth; 
    public int _currentShield; 
    public int _maxHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        playerStats.onPlayerStatsChanged(this);
    }
    private void OnEnable()
    {
        moveManager.onMoveReachedEnd.AddListener(RegisterHit);
        playerMoveManager.onMoveReachedEnd.AddListener(usePotion);
    }

    private void OnDisable()
    {
        moveManager.onMoveReachedEnd.RemoveListener(RegisterHit);
        playerMoveManager.onMoveReachedEnd.RemoveListener(usePotion);
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

    private void RegisterHit(MoveStats move)
    {
        if(_currentShield > 0 && move.damage > 0) 
        { 
            _currentShield--;
            playerStats.onPlayerStatsChanged(this); 
            return; 
        }
        _currentHealth -= move.damage;
        playerStats.onPlayerStatsChanged(this);
    }
}
