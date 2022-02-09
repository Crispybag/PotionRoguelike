using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStats : MonoBehaviour
{
    [HideInInspector] public SO_Ingredient ingredientStats;
    private SpriteRenderer spriteRenderer;

    public void Setup()
    {
        if (GetComponent<SpriteRenderer>())
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = ingredientStats.gameSprite;
        }
        gameObject.name = ingredientStats.title;

    }

    
}
