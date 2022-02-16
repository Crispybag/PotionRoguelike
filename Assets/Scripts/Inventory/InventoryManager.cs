using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private SO_Inventory inventory;
    [SerializeField] private List<SO_Ingredient> ingredientTestList;
    
    void Start()
    {
        inventory.flushData();

        for (int i = 0; i <ingredientTestList.Count; i++ )
        {
            inventory.AddToInventory(ingredientTestList[i]);
            
        }

        Autofill();
        
    }

    void Autofill()
    {
        while (inventory.inventory.Count > 0 && inventory.loadOut.Count < 5)
        {
            inventory.equipIngredient(inventory.inventory[0]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
