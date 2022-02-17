using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] List<SO_Enemy> enemy;
    [SerializeField] GameObject baseEnemyPrefab;
    [SerializeField] SO_OnEnemyStatUpdate statUpdater;


    private void OnEnable()
    {
        statUpdater.onEnemyStatUpdate.AddListener(enemyDeathSequence);
    }
    private void OnDisable()
    {
        statUpdater.onEnemyStatUpdate.RemoveListener(enemyDeathSequence);
    }

    private void Start()
    {
        spawnEnemy(Random.Range(0, enemy.Count));
    }

    void spawnEnemy(int count)
    {

        baseEnemyPrefab.GetComponent<EnemyStats>().scriptableEnemy = enemy[count];
        baseEnemyPrefab.GetComponent<EnemyStats>().Setup();
        Instantiate(baseEnemyPrefab, gameObject.transform);
    }

    void enemyDeathSequence(EnemyStats stats)
    {
        if (stats.currentHealth <= 0)
        {
            //death sequence here, expand if we want to do more with it
            spawnEnemy(Random.Range(0, enemy.Count));

        }
    }

}
