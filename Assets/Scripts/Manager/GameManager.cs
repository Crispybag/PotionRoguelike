using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<SO_Ingredient> loadOut;
    [SerializeField] private GameObject baseIngredientPrefab;

    private void Start()
    {
        if (!baseIngredientPrefab.GetComponent<IngredientStats>()) return;


        foreach(SO_Ingredient ingredient in loadOut)
        {
         
            baseIngredientPrefab.GetComponent<IngredientStats>().ingredientStats = ingredient;
            baseIngredientPrefab.GetComponent<IngredientStats>().Setup();
            baseIngredientPrefab.GetComponent<IngredientSpawning>().RespawnIngredient(baseIngredientPrefab);
            //Instantiate(baseIngredientPrefab, new Vector3(-3, 0, 0), transform.rotation);
        }
    }


}
