using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CraftingSystem/Inventory")]
public class SO_Inventory : ScriptableObject
{
    public List<SO_Ingredient> inventory = new List<SO_Ingredient>();
    public List<SO_Ingredient> loadOut = new List<SO_Ingredient>();


    public void AddToInventory(SO_Ingredient ingredient)
    {
        inventory.Add(ingredient);
    }
   
    public void RemoveFromInventory(SO_Ingredient ingredient)
    {
        inventory.Remove(ingredient);
    }

    public bool equipIngredient(SO_Ingredient inventorySlot)
    {
        //if it returns false make something happen that mentions to the player that equipment slots are full
        if (loadOut.Count >= 5) return false;

        loadOut.Add(inventorySlot);
        inventory.Remove(inventorySlot);
        return true;
               
    }
    public void reEquipIngredient(int loadoutSlot, int inventorySlot)
    {
        loadOut[loadoutSlot] = inventory[inventorySlot];
    }

    public void flushData()
    {
        inventory.Clear();
        loadOut.Clear();
    }

}
