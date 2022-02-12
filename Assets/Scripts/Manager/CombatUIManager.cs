using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CombatUIManager : MonoBehaviour
{
    [SerializeField] SO_OnEnemyStatUpdate enemyStatManager;
    [SerializeField] SO_PlayerStats playerManager;

    [SerializeField] Text playerHealthtext;
    [SerializeField] Text playerShieldtext;
    [SerializeField] Text enemyHealthtext;
    [SerializeField] Text enemyShieldtext;


    private void OnEnable()
    {
        enemyStatManager.onEnemyStatUpdate.AddListener(UpdateTextEnemy);
        playerManager.onStatsChanged.AddListener(UpdateTextPlayer);
    }

    private void OnDisable()
    {
        enemyStatManager.onEnemyStatUpdate.RemoveListener(UpdateTextEnemy);
        playerManager.onStatsChanged.RemoveListener(UpdateTextPlayer);
    }

    void UpdateTextEnemy(EnemyStats enemy)
    {
        enemyHealthtext.text = enemy.currentHealth.ToString();
        enemyShieldtext.text = enemy.currentShield.ToString();
    }

    void UpdateTextPlayer(PlayerManager player)
    {
        playerHealthtext.text = player._currentHealth.ToString();
        playerShieldtext.text = player._currentShield.ToString();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
