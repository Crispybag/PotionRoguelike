using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{

    public List<SO_Recipe> recipes;
    private Dictionary<GameObject, int> currentIngredients = new Dictionary<GameObject, int>();

    private List<SO_Recipe> possibleRecipes = new List<SO_Recipe>();
    private List<SO_Recipe> craftableRecipes = new List<SO_Recipe>();

    /// <summary>
    /// Updates the current craftable recipe (if there is any)
    /// </summary>
    public void UpdateRecipe()
    {
        checkCraftableRecipes();
        checkPossibleRecipes();
    }


    private void checkCraftableRecipes()
    {
        craftableRecipes.Clear();
        //loop through all recipes
        foreach(SO_Recipe recipe in recipes)
        {
            //check if the recipe can be crafted
            if (recipe.canCraft(currentIngredients))
            {
                //if so add it to the list
                craftableRecipes.Add(recipe);
            }
        }
        //a check if there are more than 1 possible crafting recipe, this is not allowed.
        if(craftableRecipes.Count > 1)
        {
            Debug.LogWarning(craftableRecipes[0].title + " and " + craftableRecipes[1].title + " have the same crafting recipe, this is not allowed, please solve this");
            return;
        }
        else if(craftableRecipes.Count > 0)
        {
            Debug.Log("Found a recipe that can be crafted! : " + craftableRecipes[0].title);
            return;
        }
    }


    /// <summary>
    /// Checks if there are still possible recipes, and puts these in a list (possibleRecipes)
    /// </summary>
    public bool checkPossibleRecipes()
    {
        possibleRecipes.Clear();
        foreach(SO_Recipe recipe in recipes)
        {
            if (recipe.canStillBeCrafted(currentIngredients))
            {
                possibleRecipes.Add(recipe);
            }
        }
        if(possibleRecipes.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// Use this method to add an ingredient, this will update the dictionary correctly.
    /// </summary>
    /// <param name="ingredient"> The ingredient you want to add </param>
    public void AddIngredient(GameObject ingredient)
    {
        //check if there is a dictionary
        if (currentIngredients == null) return;
        //if the dictionary does not contain a key for the ingredient yet, create it, with value 1 (because there is one of that ingredient so far)
        if (!currentIngredients.ContainsKey(ingredient))
        {
            currentIngredients.Add(ingredient, 1);
        }
        //if it does exist, increase the value with one
        else
        {
            currentIngredients[ingredient] = currentIngredients[ingredient] + 1;
        }
        //since we adjusted the current ingredients, the result can be different now.
        UpdateRecipe();
    }


    /// <summary>
    /// clears all ingredients and possible/craftable recipes
    /// </summary>
    public void ClearIngredients()
    {
        currentIngredients.Clear();
        possibleRecipes.Clear();
        craftableRecipes.Clear();
    }



}
