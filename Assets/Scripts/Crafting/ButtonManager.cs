using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public CraftingManager craftingManager;



    public void AddIngredient(GameObject ingredient)
    {
        craftingManager.AddIngredient(ingredient);
    }

    public void ClearIngredients()
    {
        //craftingManager.currentIngredients.Clear();
    }


}
