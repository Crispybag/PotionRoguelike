using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "CraftingSystem/Recipe")]
public class SO_Recipe : ScriptableObject
{
    public string title;
    public List<SO_Ingredient> ingredients;
    public SO_Potion potion;
    private Dictionary<string, int> ingredientCount = new Dictionary<string, int>();


    private void OnValidate()
    {
        ingredientCount.Clear();
        //loop through each ingredient
        foreach(SO_Ingredient ingredient in ingredients)
        {
            //if it doesnt exist in the dictionary yet, add it, with value 1
            if (!ingredientCount.ContainsKey(ingredient.title))
            {
                ingredientCount.Add(ingredient.title, 1);
            }
            //if it does exist, get its value, and increase it by one
            else
            {
                ingredientCount[ingredient.title] = ingredientCount[ingredient.title] + 1;
            }
        }
    }

    public bool hasIngredient(string ingredientName)
    {
        foreach(KeyValuePair<string, int> ingredient in ingredientCount)
        {
            if(ingredientName == ingredient.Key)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Checks if the recipe is craftable
    /// </summary>
    /// <param name="currentIngredients"> The current ingredients in the pot. </param>
    /// <returns></returns>
    public bool canCraft(Dictionary<string, int> currentIngredients)
    {
        //check if the dictionary sizes are the same size, else we know already it cant be crafted.
        if(currentIngredients.Count != ingredientCount.Count)
        {
            return false;
        }
        //then loop through all the ingredients of the given dictionary
        foreach(KeyValuePair<string, int> ingredient in currentIngredients)
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


    public bool canStillBeCrafted(Dictionary<string, int> currentIngredients)
    {
        //check if the dictionary size is bigger than the ingredients needed, if so, its uncraftable.
        if (currentIngredients.Count > ingredientCount.Count)
        {
            return false;
        }
        //loop through all the ingredients of the given dictionary
        foreach (KeyValuePair<string, int> ingredient in currentIngredients)
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
