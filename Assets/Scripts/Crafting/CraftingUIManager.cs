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
        foreach(GameObject ingredient in craftingManager.correctOrderIngredients)
        {
            GameObject ingr = Instantiate(ingredientPrefab);
            ingr.transform.SetParent(craftingPot.transform);
            currentIngredients.Add(ingr);
            //ingr.GetComponent<Image>().sprite = ingredient.GetComponent<SpriteRenderer>().sprite;
        }
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
