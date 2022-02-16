using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CraftingSystem/Inventory")]
public class SO_Inventory : ScriptableObject
{
    public List<SO_Ingredient> inventory = new List<SO_Ingredient>();
    public List<SO_Ingredient> loadOut = new List<SO_Ingredient>();
    public SO_RecipeBook recipeBook;
    public List<SO_Recipe> loadOutRecipes;
    public List<SO_Recipe> inventoryRecipes;
    public void AddToInventory(SO_Ingredient ingredient)
    {
        inventory.Add(ingredient);
        //updateCurrentRecipes();

    }

    public void RemoveFromInventory(SO_Ingredient ingredient)
    {
        inventory.Remove(ingredient);
        //updateCurrentRecipes();

    }

    public bool equipIngredient(SO_Ingredient inventorySlot)
    {
        //if it returns false make something happen that mentions to the player that equipment slots are full
        if (loadOut.Count >= 5) return false;

        loadOut.Add(inventorySlot);
        inventory.Remove(inventorySlot);
        //updateCurrentRecipes();
        return true;


    }
    public void reEquipIngredient(int loadoutSlot, int inventorySlot)
    {
        loadOut[loadoutSlot] = inventory[inventorySlot];
        //updateCurrentRecipes();

    }

    public void flushData()
    {
        inventory.Clear();
        loadOut.Clear(); 
        //updateCurrentRecipes();
    }

    public void updateCurrentRecipes()
    {
        List<SO_Ingredient> allIngredients = new List<SO_Ingredient>();
        allIngredients.AddRange(inventory);
        allIngredients.AddRange(loadOut);

        //determine all craftable recipes with ingredients from loadout and inventory combined
        inventoryRecipes = setCraftableRecipes(allIngredients);

        //determine all craftable recipes with ingredients from load out only
        loadOutRecipes = setCraftableRecipes(loadOut);

        //remove all already possible loadouts in load out from the inventory recipes
        foreach (SO_Recipe recipe in loadOutRecipes)
        {
            inventoryRecipes.Remove(recipe);
        }
    }


    private List<SO_Recipe> setCraftableRecipes(List<SO_Ingredient> ingredients)
    {
        List<SO_Recipe> craftableRecipes = new List<SO_Recipe>();

        foreach(SO_Recipe recipe in recipeBook.inGameRecipes)
        {
                if (recipe.hasIngredients(ingredients))
                {
                craftableRecipes.Add(recipe);
                continue;
                }
        }
        return craftableRecipes;
    }

}
