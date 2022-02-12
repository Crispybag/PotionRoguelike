using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] SO_Enemy enemy;
    [SerializeField] GameObject baseEnemyPrefab;

    private void Start()
    {
        spawnEnemy();
    }

    void spawnEnemy()
    {
        baseEnemyPrefab.GetComponent<EnemyStats>().scriptableEnemy = enemy;
        baseEnemyPrefab.GetComponent<EnemyStats>().Setup();
        Instantiate(baseEnemyPrefab, gameObject.transform);
    }
}
