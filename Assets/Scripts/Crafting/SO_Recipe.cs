using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe")]
public class SO_Recipe : ScriptableObject
{
    public string title;
    public List<GameObject> ingredients;
    private Dictionary<GameObject, int> ingredientCount = new Dictionary<GameObject, int>();

    private void OnValidate()
    {
        ingredientCount.Clear();
        //loop through each ingredient
        foreach(GameObject ingredient in ingredients)
        {
            //if it doesnt exist in the dictionary yet, add it, with value 1
            if (!ingredientCount.ContainsKey(ingredient))
            {
                ingredientCount.Add(ingredient, 1);
            }
            //if it does exist, get its value, and increase it by one
            else
            {
                ingredientCount[ingredient] = ingredientCount[ingredient] + 1;
            }
        }
    }


    /// <summary>
    /// Checks if the recipe is craftable
    /// </summary>
    /// <param name="currentIngredients"> The current ingredients in the pot. </param>
    /// <returns></returns>
    public bool canCraft(Dictionary<GameObject, int> currentIngredients)
    {
        //check if the dictionary sizes are the same size, else we know already it cant be crafted.
        if(currentIngredients.Count != ingredientCount.Count)
        {
            return false;
        }
        //then loop through all the ingredients of the given dictionary
        foreach(KeyValuePair<GameObject, int> ingredient in currentIngredients)
        {
            //if it doesnt contain the ingredient, then we know it isnt craftable already.
            if (!ingredientCount.ContainsKey(ingredient.Key))
            {
                return false;
            }
            //if the size of the ingredients isnt the same, it neither can be crafted
            if(ingredientCount[ingredient.Key] != currentIngredients[ingredient.Key])
            {
                return false;
            }
        }
        //it got through all the checks, thus it can be crafted! :D
        return true;
    }


    public bool canStillBeCrafted(Dictionary<GameObject, int> currentIngredients)
    {
        //check if the dictionary size is bigger than the ingredients needed, if so, its uncraftable.
        if (currentIngredients.Count > ingredientCount.Count)
        {
            return false;
        }
        //loop through all the ingredients of the given dictionary
        foreach (KeyValuePair<GameObject, int> ingredient in currentIngredients)
        {
            //if it doesnt contain the ingredient, then we know it isnt craftable already.
            if (!ingredientCount.ContainsKey(ingredient.Key))
            {
                return false;
            }
            //if the size of the ingredients is higher than thats needed, return false, we are already over limit
            if (ingredientCount[ingredient.Key] < currentIngredients[ingredient.Key])
            {
                return false;
            }
        }
        //it got through all the checks, thus it still can be crafted! :D
        return true;
    }


}
