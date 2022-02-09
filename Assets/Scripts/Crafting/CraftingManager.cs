using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{

    public List<SO_Recipe> recipes;
    private Dictionary<string, int> currentIngredients = new Dictionary<string, int>();

    private List<SO_Recipe> possibleRecipes = new List<SO_Recipe>();
    private List<SO_Recipe> craftableRecipes = new List<SO_Recipe>();
    private SO_Recipe currentRecipe;
    //-1 because it means no crafting recipe has been set yet
    private int recipeTier =  -1;

    /// <summary>
    /// Updates the current craftable recipe (if there is any)
    /// </summary>
    public void UpdateRecipe()
    {

        checkPossibleRecipes();
        checkCraftableRecipes();
    }




    public void Update()
    {
        //reset crafting
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ClearIngredients();
        }
        //confirm crafting or throw potion
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(currentRecipe == null){
                setCraftingRecipe();
            }
            else
            {

            }
        }
    }

    private void setCraftingRecipe()
    {
        //set crafting
        if (craftableRecipes.Count == 1)
        {
            currentRecipe = craftableRecipes[0];
            currentIngredients.Clear();
            recipeTier++;
        }
    }


    private void checkCraftableRecipes()
    {
        craftableRecipes.Clear();
        //check if there is a possible recipe
        if (possibleRecipes == null || possibleRecipes.Count == 0) return;
        //loop through all recipes
        foreach(SO_Recipe recipe in possibleRecipes)
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
            Debug.LogError(craftableRecipes[0].title + " and " + craftableRecipes[1].title + " have the same crafting recipe, this is not allowed, please solve this");
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

        if (currentRecipe != null)
        {
            AddTierRecipe(ingredient);
            return;
        }
        //if the dictionary does not contain a key for the ingredient yet, create it, with value 1 (because there is one of that ingredient so far)
        if (!currentIngredients.ContainsKey(ingredient.name))
        {
            currentIngredients.Add(ingredient.name, 1);
        }
        //if it does exist, increase the value with one
        else
        {
            currentIngredients[ingredient.name] = currentIngredients[ingredient.name] + 1;
        }
        //since we adjusted the current ingredients, the result can be different now.
        UpdateRecipe();
    }

    public void AddTierRecipe(GameObject ingredient)
    {
        //check if the ingredient is an ingredient in current recipe, if not, clear the recipe and everything else
        if(!currentRecipe.hasIngredient(ingredient.name))
        {
            Debug.Log("Incorrect crafting material to tier up, clearing crafting...");
            ClearIngredients();
            return;
        }
        //if the dictionary does not contain a key for the ingredient yet, create it, with value 1 (because there is one of that ingredient so far)
        if (!currentIngredients.ContainsKey(ingredient.name))
        {
            currentIngredients.Add(ingredient.name, 1);
        }
        //if it does exist, increase the value with one
        else
        {
            currentIngredients[ingredient.name] = currentIngredients[ingredient.name] + 1;
        }
        //check if we can craft the recipe, if so increase the tier
        if (currentRecipe.canCraft(currentIngredients))
        {
            recipeTier++;
            currentIngredients.Clear();
            Debug.Log("Tier of crafting just went up... now tier : " + recipeTier);
        }
        //if we cant craft it, check if we still can craft it, if not, destroy recipe and clear ingredients
        if (!currentRecipe.canStillBeCrafted(currentIngredients))
        {
            Debug.Log("Incorrect crafting material to tier up, clearing crafting...");
            ClearIngredients();
            return;
        }

    }



    /// <summary>
    /// Clears all ingredients and possible/craftable recipes
    /// </summary>
    public void ClearIngredients()
    {
        currentRecipe = null;
        recipeTier = -1;
        currentIngredients.Clear();
        possibleRecipes.Clear();
        craftableRecipes.Clear();
    }



}
