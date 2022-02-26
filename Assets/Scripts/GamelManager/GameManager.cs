using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SO_Inventory inventory;
    [SerializeField] private SO_OnGridManagerChanged gridManagerChanged;
    private List<SO_Ingredient> loadOut = new List<SO_Ingredient>();
    [SerializeField] private GameObject baseIngredientPrefab;
    private GridManager _gridManager;
    private void Start()
    {
        if (!baseIngredientPrefab.GetComponent<IngredientStats>()) return;

        
        foreach (SO_Ingredient ingredient in inventory.loadOut)
        { loadOut.Add(ingredient); }

        foreach(SO_Ingredient ingredient in loadOut)
        {
            if (ingredient == null)
            {
                continue;
            }
            baseIngredientPrefab.GetComponent<IngredientStats>().ingredientStats = ingredient;
            baseIngredientPrefab.GetComponent<IngredientStats>().Setup();
            if (gridManagerChanged.OnRequestGridManager()) _gridManager.SpawnGridObject(baseIngredientPrefab);
            //Instantiate(baseIngredientPrefab, new Vector3(-3, 0, 0), transform.rotation);
        }
    }

    private void GetGridManager(GridManager pGridManager)
    {
        _gridManager = pGridManager;
    }

    private void OnEnable()
    {
        gridManagerChanged.onGridManagerChanged.AddListener(GetGridManager);
    }
    
    private void OnDisable()
    {
        gridManagerChanged.onGridManagerChanged.RemoveListener(GetGridManager);
    }


}
