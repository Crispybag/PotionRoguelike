using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static ServiceLocator serviceLocator;
    private Dictionary<string, GameObject> gameObjectList;

    private void Awake()
    {
        if (serviceLocator != null) { Destroy(this.gameObject); return; }
        serviceLocator = this;

    }

    public static GameObject GetFromList(string key)
    {
        if (serviceLocator.gameObjectList.ContainsKey(key))
        {
            return serviceLocator.gameObjectList[key];
        }

        else
        {
            return null;
        }
    }

    public static void AddToList(string key, GameObject val)
    {
        serviceLocator.gameObjectList.Add(key, val);
    }

    public static void RemoveFromList(string key)
    {
        if (serviceLocator.gameObjectList.ContainsKey(key))
        {
            serviceLocator.gameObjectList.Remove(key);
        }
    }

}
