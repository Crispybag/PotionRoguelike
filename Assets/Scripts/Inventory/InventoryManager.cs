using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private SO_Inventory inventory;
    [SerializeField] private List<SO_Ingredient> ingredientTestList;
    [SerializeField] private SO_RecipeBook recipeBook;
    [SerializeField] private int inventorySize = 40;
    //temporary for testing
    void Start()
    {
        Debug.LogWarning("Temporary Code! Remove when done testing!");

        inventory.flushData();
        inventory.createInventorySize(inventorySize);
        for (int i = 0; i <ingredientTestList.Count; i++ )
        {
            inventory.AddToInventory(ingredientTestList[i]);
            
        }

        Autofill();
        inventory.updateCurrentRecipes();
    }

    //kind of temporary, automatically fills the equipment out for the player
    void Autofill()
    {
        for(int i = 0; i < 5; i++)
        {
            inventory.reEquipIngredient(i, i);
        }
    }

    //button interface to refresh the ingredients
    public void FillRecipeBook()
    {
        recipeBook.fillGameRecipes();
    }
}
