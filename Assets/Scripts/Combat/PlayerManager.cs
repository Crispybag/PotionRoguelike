using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //build observer
    public int  _currentHealth, _currentShield;
    [SerializeField] private SO_PlayerStats playerStats;

    [SerializeField] private SO_MoveTriggerManager moveManager;
    private void TakeDamage(int pDmg)
    {
        if (_currentShield > 0)
        {
            _currentShield--;
            return;
        }
        _currentHealth -= pDmg;
    }


    private void OnEnable()
    {
        moveManager.onMoveReachedEnd.AddListener(RegisterHit);
    }

    private void OnDisable()
    {
        moveManager.onMoveReachedEnd.RemoveListener(RegisterHit);

    }
    private void RegisterHit(MoveStats move)
    {
        if(_currentShield > 0 && move.damage > 0) { _currentShield--; return; }
        _currentHealth -= move.damage;
        playerStats.onPlayerStatsChanged(this);
    }
}
