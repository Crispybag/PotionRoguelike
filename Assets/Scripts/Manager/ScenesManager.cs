using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class ScenesManager : MonoBehaviour
{
    int playerHealth = 1;
    [SerializeField] SO_PlayerStats playerStats;
    void Start()
    {
        
    }


    private void OnEnable()
    {
        playerStats.onStatsChanged.AddListener(checkForPlayerHealth);
    }

    private void OnDisable()
    {
        playerStats.onStatsChanged.RemoveListener(checkForPlayerHealth);

    }


    void checkForPlayerHealth(PlayerManager manager)
    {
        playerHealth = manager._currentHealth;
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            LoadScene(1);
        }
    }
}
