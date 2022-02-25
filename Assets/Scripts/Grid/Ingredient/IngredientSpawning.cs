using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class IngredientSpawning : MonoBehaviour
{

    
    [SerializeField] private int amountOfGarbage = 1;  
    public void OnGameObjectDestroy()
    {
        GridManager.mapManager.SpawnGridObject(gameObject);
        for (int i = 0; i < amountOfGarbage; i++)
        {
            GridManager.mapManager.SpawnGridObject(GridManager.mapManager.GetGarbagePrefab());
        }
    }
}
