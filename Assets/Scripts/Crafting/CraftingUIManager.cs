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

    public void Start()
    {
        craftingManager = FindObjectOfType<CraftingManager>();
        CraftingManager.onIngredientUpdate += UpdateIngredients;
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


    

}
