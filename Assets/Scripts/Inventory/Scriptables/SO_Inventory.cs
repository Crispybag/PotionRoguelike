using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CraftingSystem/Inventory")]
public class SO_Inventory : ScriptableObject
{
    //what is in inventory
    public List<SO_Ingredient> inventory = new List<SO_Ingredient>();

    //what is in the loadout
    public List<SO_Ingredient> loadOut = new List<SO_Ingredient>();

    //Reference to the recipe book
    public SO_RecipeBook recipeBook;

    //recipes you can make with the loadout
    public List<SO_Recipe> loadOutRecipes;

    //recipes you can make from loadout and inventory
    public List<SO_Recipe> inventoryRecipes;



    public void createInventorySize(int ammount)
    {
        for (int i = 0; i < ammount; i++)
        {
            inventory.Add(null);
        }
        for(int i = 0; i < 5; i++)
        {
            loadOut.Add(null);
        }
    }

    /// <summary>
    /// Adds an ingredient to the inventory
    /// </summary>
    public void AddToInventory(SO_Ingredient ingredient)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = ingredient;
                break;
            }
        }


        updateCurrentRecipes();

    }

    /// <summary>
    /// Removes an item from the inventory
    /// </summary>
    public void RemoveFromInventory(SO_Ingredient ingredient)
    {
        for(int i = 0; i <  inventory.Count; i++)  
        {
            if (inventory[i] == ingredient)
            {
                inventory[i] = null;
                break;
            }
        }
        updateCurrentRecipes();

    }

    /// <summary>
    /// Equip an ingredient to the current loadout
    /// </summary>
    /// <returns> checks if the equip is valid </returns>
    public bool equipIngredient(SO_Ingredient inventorySlot)
    {
        //if it returns false make something happen that mentions to the player that equipment slots are full
        if (loadOut.Count >= 5) return false;

        loadOut.Add(inventorySlot);
        RemoveFromInventory(inventorySlot);
        updateCurrentRecipes();
        return true;
    }

    /// <summary>
    /// swap an ingredient from the loadout with one in the inventory
    /// </summary>
    /// <param name="loadoutSlot"></param>
    /// <param name="inventorySlot"></param>
    public void reEquipIngredient(int loadoutSlot, int inventorySlot)
    {
        if (loadoutSlot >= loadOut.Count || inventorySlot >= inventory.Count) {Debug.LogWarning("Trying to access a too high array count"); return; }


        SO_Ingredient loadOutIngredient = loadOut[loadoutSlot];
        loadOut[loadoutSlot] = inventory[inventorySlot];
        inventory[inventorySlot] = loadOutIngredient;
        updateCurrentRecipes();

    }

    public void swapPositions(List<SO_Ingredient> list, int pos1, int pos2)
    {
        if (list.Count < pos1 || list.Count < pos2) { Debug.LogWarning("Trying to access a too high array count"); return; }
        SO_Ingredient loadOutIngredient = list[pos1];
        list[pos1] = list[pos2];
        list[pos2] = loadOutIngredient;
    }


    /// <summary>
    /// clear the data for quick resetting
    /// </summary>
    public void flushData()
    {
        inventory.Clear();
        loadOut.Clear(); 
        updateCurrentRecipes();
    }

    /// <summary>
    /// update the recipes based on what ingredients you have
    /// </summary>
    public void updateCurrentRecipes()
    {
        //all ingredients you currently have equipped
        List<SO_Ingredient> allIngredients = new List<SO_Ingredient>();

        //add both inventory and loadout to the equip
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

    /// <summary>
    /// Determines what recipes are craftable and what aren't
    /// </summary>
    private List<SO_Recipe> setCraftableRecipes(List<SO_Ingredient> ingredients)
    {
        //return value
        List<SO_Recipe> craftableRecipes = new List<SO_Recipe>();

        //check for each ingredient whether it has the correct ingredients or not
        foreach(SO_Recipe recipe in recipeBook.inGameRecipes)
        {
                if (recipe.hasIngredients(ingredients))
                {
                //if successful, go to the next recipe and add to list
                craftableRecipes.Add(recipe);
                continue;
                }
        }
        return craftableRecipes;
    }

}
