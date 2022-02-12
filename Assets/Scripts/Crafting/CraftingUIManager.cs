using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MonoBehaviour
{

    public GameObject craftingPot;
    public CraftingManager craftingManager;
    public List<GameObject> currentIngredients;
    public GameObject ingredientPrefab;
    public List<GameObject> tierBlocks;

    public void Start()
    {
        craftingManager = FindObjectOfType<CraftingManager>();
        CraftingManager.onIngredientUpdate += UpdateIngredients;
        CraftingManager.onTierUpdate += UpdateTier;
    }

    public void UpdateIngredients()
    {
        ClearIngredients();
        List<SO_Ingredient> curIngredients = new List<SO_Ingredient>(craftingManager.correctOrderIngredients);

        if(craftingManager.recipeTier != -1)
        {
            foreach (SO_Ingredient ingredient in craftingManager.currentRecipe.ingredients)
            {
                GameObject ingr = Instantiate(ingredientPrefab);
                ingr.transform.SetParent(craftingPot.transform);
                currentIngredients.Add(ingr);
                ingr.name = ingredient.title;
                ingr.GetComponent<Image>().sprite = ingredient.icon;

                SetAlpha(ingr, 0.25f);

                List<SO_Ingredient> ingredientsToRemove = new List<SO_Ingredient>();
                foreach (SO_Ingredient curIngr in curIngredients)
                {
                    if (curIngr.title == ingredient.title)
                    {
                        SetAlpha(ingr, 1);
                        ingredientsToRemove.Add(curIngr);
                        break;
                    }
                }
                foreach (SO_Ingredient ingrToRemove in ingredientsToRemove)
                {
                    curIngredients.Remove(ingrToRemove);
                }
            }




        }
        else
        {
            foreach (SO_Ingredient ingredient in craftingManager.correctOrderIngredients)
            {
                GameObject ingr = Instantiate(ingredientPrefab);
                ingr.transform.SetParent(craftingPot.transform);
                currentIngredients.Add(ingr);
                ingr.GetComponent<Image>().sprite = ingredient.icon;
            }
        }

    }

    private void SetAlpha(GameObject obj, float alpha)
    {
        Color newColor = obj.GetComponent<Image>().color;
        newColor.a = alpha;
        obj.GetComponent<Image>().color = newColor;
    }

    private void ClearIngredients()
    {
        foreach (GameObject ingr in currentIngredients)
        {
            Destroy(ingr.gameObject);
        }
        currentIngredients.Clear();
    }






    private void UpdateTier()
    {
        DisableTiers();
        for(int i = 0; i <=  craftingManager.recipeTier; i++)
        {
            Debug.Log("updating tiers....");
            Color newColor = tierBlocks[i].GetComponent<Image>().color;
            newColor.a = 1f;
            tierBlocks[i].GetComponent<Image>().color = newColor;
        }
    }

    private void DisableTiers()
    {
        foreach(GameObject tier in tierBlocks)
        {
            Color newColor = tier.GetComponent<Image>().color;
            newColor.a = 0.5f;
            tier.GetComponent<Image>().color = newColor;
        }
    }

}
