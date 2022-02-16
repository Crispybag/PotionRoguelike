using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private SO_Inventory inventory;
    [SerializeField] private List<SO_Ingredient> ingredientTestList;
    [SerializeField] private SO_RecipeBook recipeBook;
    
    void Start()
    {
        inventory.flushData();

        for (int i = 0; i <ingredientTestList.Count; i++ )
        {
            inventory.AddToInventory(ingredientTestList[i]);
            
        }

        Autofill();
        inventory.updateCurrentRecipes();
    }

    void Autofill()
    {
        while (inventory.inventory.Count > 0 && inventory.loadOut.Count < 5)
        {
            inventory.equipIngredient(inventory.inventory[0]);
        }
    }
    public void FillRecipeBook()
    {
        recipeBook.fillGameRecipes();
    }
}
