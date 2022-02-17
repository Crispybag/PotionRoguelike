using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class ScenesManager : MonoBehaviour
{
    public static void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    public static void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
