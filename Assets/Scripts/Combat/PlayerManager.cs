using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GetPlayerStats(int pHealth, int pShield);
public class PlayerManager : MonoBehaviour
{
    //build observer
    public static event GetPlayerStats getPlayerStats;
    private int _oldHealth, _oldShield, _currentHealth, _currentShield;

    private void TakeDamage(int pDmg)
    {
        if (_currentShield > 0)
        {
            _currentShield--;
            return;
        }
        _currentHealth -= pDmg;
    }

    public bool IsPlayerHit()
    {
        //if we want to add some form of evasiveness we can add it here
        //bullets can also apply effects when they hit by getting whether the bullet hit or not
        return true;
    }

    private void Update()
    {
        //send out message when stats change
        if(_oldHealth != _currentHealth || _oldShield != _currentShield)
        {
            getPlayerStats(_currentHealth, _currentShield);
        }
        _oldShield = _currentShield;
        _oldHealth = _currentHealth;
    }
}
