using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Manager/OnEnemyStatUpdate")]
public class SO_OnEnemyStatUpdate : ScriptableObject
{
    public UnityEvent<EnemyStats> onEnemyStatUpdate;

    public void onEnemyStatsChanged(EnemyStats enemyStats)
    {
       onEnemyStatUpdate.Invoke(enemyStats);
    }

}
