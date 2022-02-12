using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void IngredientUpdateHandler();
public delegate void TierUpdateHandler();

public class CraftingManager : MonoBehaviour
{
    public List<SO_Recipe> recipes;
    private Dictionary<string, int> currentIngredients = new Dictionary<string, int>();
    public List<SO_Ingredient> correctOrderIngredients = new List<SO_Ingredient>();

    public static IngredientUpdateHandler onIngredientUpdate;
    public static TierUpdateHandler onTierUpdate;

    private List<SO_Recipe> possibleRecipes = new List<SO_Recipe>();
    private List<SO_Recipe> craftableRecipes = new List<SO_Recipe>();
    [HideInInspector] public SO_Recipe currentRecipe;
    //-1 because it means no crafting recipe has been set yet
    public int recipeTier = -1;

    //variables for crafting potions
    public SO_PlayerMoveManager moveManager;
    public GameObject potionPrefab;


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
                ThrowPotion();
            }
        }
    }

    private void setCraftingRecipe()
    {
        //set crafting
        if (craftableRecipes.Count == 1)
        {
            currentRecipe = craftableRecipes[0];
            increaseTier();
        }
        onIngredientUpdate();
    }

    private void increaseTier()
    {
        currentIngredients.Clear();
        correctOrderIngredients.Clear();
        onIngredientUpdate();
        recipeTier++;
        //4 because we start counting at 0
        if (recipeTier == 4)
        {
            ThrowPotion();

        }
        onTierUpdate();
        Debug.Log("Tier of crafting just went up... now tier : " + recipeTier);
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
            ClearIngredients();
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

        if (!ingredient.GetComponent<IngredientStats>())
        {
            ClearIngredients();
            return;
        }

        if (currentRecipe != null)
        {
            AddTierRecipe(ingredient);
            return;
        }
        //add ingredient to current ingredient list
        AddIngredientList(ingredient);
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
        //add ingredient to current ingredient list
        AddIngredientList(ingredient);
        //check if we can craft the recipe, if so increase the tier
        if (currentRecipe.canCraft(currentIngredients))
        {
            increaseTier();
            return;
        }
        //if we cant craft it, check if we still can craft it, if not, destroy recipe and clear ingredients
        if (!currentRecipe.canStillBeCrafted(currentIngredients))
        {
            Debug.Log("Incorrect crafting material to tier up, clearing crafting...");
            ClearIngredients();
            return;
        }

    }

    private void ThrowPotion()
    {
        //Ayo o/ Leo Here, potion will now be thrown based on the potion linked to the recipe and the tier
        Instantiate(potionPrefab);
        //pass through the potion and the crafting manager for stats
        potionPrefab.GetComponent<PotionStats>().Setup(currentRecipe.potion, this);


        ClearIngredients();
    }

    private void AddIngredientList(GameObject ingredient)
    {
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
        correctOrderIngredients.Add(ingredient.GetComponent<IngredientStats>().ingredientStats);
        onIngredientUpdate();
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
        correctOrderIngredients.Clear();
        onIngredientUpdate();
        onTierUpdate();
    }

    //flush delegate when reloading scene
    private void OnDisable()
    {
        onIngredientUpdate = null;
        onTierUpdate = null;
    }


}
