using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private SO_Inventory inventory;
    [SerializeField] private List<SO_Ingredient> ingredientTestList;
    [SerializeField] private SO_RecipeBook recipeBook;
    
    //temporary for testing
    void Start()
    {
        Debug.LogWarning("Temporary Code! Remove when done testing!");

        inventory.flushData();

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
        while (inventory.inventory.Count > 0 && inventory.loadOut.Count < 5)
        {
            inventory.equipIngredient(inventory.inventory[0]);
        }
    }

    //button interface to refresh the ingredients
    public void FillRecipeBook()
    {
        recipeBook.fillGameRecipes();
    }


    
    private void Update()
    {
        //REMOVE THIS WHEN DONE WITH TESTING!!!
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.LogWarning("Temporary Code! Remove when done testing!");
            inventory.reEquipIngredient(0, 0);
        }
    }
}
