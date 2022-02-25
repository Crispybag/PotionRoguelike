using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SO_Inventory inventory;
    private List<SO_Ingredient> loadOut = new List<SO_Ingredient>();
    [SerializeField] private GameObject baseIngredientPrefab;

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
            GridManager.mapManager.SpawnGridObject(baseIngredientPrefab);
            //Instantiate(baseIngredientPrefab, new Vector3(-3, 0, 0), transform.rotation);
        }
    }


}
