using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTile : MonoBehaviour
{
    [SerializeField] SO_InventorySelector inventorySelector;
    [SerializeField] SO_Inventory inventory;
    [SerializeField] private int inventoryOffset = 2;
    private int lastInventoryTile = -100;

    private void getIngredientOnTile(SelectorMovement movement)
    {
        int currentLocation = (int)(movement.mapPosition.y) * movement._map.size.x + (int)movement.mapPosition.x;

        //if a tile hasnt been selected yet
        if (lastInventoryTile == -100)
        {
            lastInventoryTile = currentLocation;
        }

        //else if one tile is load out and other is inventory
        else
        {
            swapPositions(movement, currentLocation);
        }


    }


    private void swapPositions(SelectorMovement movement, int currentLocation)
    {
        //Swap between inventory and loadout
        if (lastInventoryTile < movement._map.size.x ^ currentLocation < movement._map.size.x)
        {
            if (lastInventoryTile < 5)
            {
                //swap 2, account for inventory offset
                inventory.reEquipIngredient(lastInventoryTile, currentLocation - movement._map.size.x * inventoryOffset);
                lastInventoryTile = -100;
            }
            else
            {
                //swap 2, account for inventory offset
                inventory.reEquipIngredient(currentLocation, lastInventoryTile - movement._map.size.x * inventoryOffset);
                lastInventoryTile = -100;
            }
        }

        //swap within inventory or loadout
        //if both tiles are in the equipment
        else if (lastInventoryTile < movement._map.size.x && currentLocation < movement._map.size.x)
        {
            inventory.swapPositions(inventory.loadOut, currentLocation, lastInventoryTile);
            lastInventoryTile = -100;
        }

        //other cases (inventory
        else
        {
            //swap 2, account for inventory offset
            inventory.swapPositions(inventory.inventory, currentLocation - movement._map.size.x * inventoryOffset, lastInventoryTile - movement._map.size.x * inventoryOffset);
            lastInventoryTile = -100;
        }

    }

    private void OnEnable()
    {
        inventorySelector.onCursorSelected.AddListener(getIngredientOnTile);
    }

    private void OnDisable()
    {
        inventorySelector.onCursorSelected.RemoveListener(getIngredientOnTile);
    }
}
