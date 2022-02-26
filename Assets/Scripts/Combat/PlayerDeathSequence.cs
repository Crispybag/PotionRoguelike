using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathSequence : MonoBehaviour
{
    [SerializeField] SO_PlayerStats playerStats;
    [SerializeField] GameObject deathScreen;
    private void OnEnable()
    {
        playerStats.onStatsChanged.AddListener(onPlayerDeath);
    }
    private void OnDisable()
    {
        playerStats.onStatsChanged.RemoveListener(onPlayerDeath);
    }

    //call this function when player's health is changed
    void onPlayerDeath(PlayerManager playerManager)
    {
        if (playerManager.currentHealth <= 0)
        {
            startEndSequence();
        }
    }

    //call this function to work on end sequence, expand this function if the player has died.
    void startEndSequence()
    {
        deathScreen.SetActive(true);
    }    




}
