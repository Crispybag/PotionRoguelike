using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class IngredientSpawning : MonoBehaviour
{

    [SerializeField] private SO_OnGridManagerChanged onGridManager;
    private GridManager gridManager;
    [SerializeField] private int amountOfGarbage = 1;  
    public void OnGameObjectDestroy()
    {
        if (onGridManager.OnRequestGridManager())
        {
            gridManager.SpawnGridObject(gameObject);
            for (int i = 0; i < amountOfGarbage; i++)
            {
                gridManager.SpawnGridObject(gridManager.GetGarbagePrefab());
            }
        }
    }


    private void OnEnable()
    {
        onGridManager.onGridManagerChanged.AddListener(getGridManager);
    }
    private void OnDisable()
    {
        onGridManager.onGridManagerChanged.RemoveListener(getGridManager);
    }

    private void getGridManager(GridManager pGridManager)
    {
        gridManager = pGridManager;
    }

}
