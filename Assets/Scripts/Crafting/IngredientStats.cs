using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStats : MonoBehaviour
{
    public SO_Ingredient ingredientStats;
    private SpriteRenderer spriteRenderer;

    public void Setup()
    {
        if (GetComponent<SpriteRenderer>())
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            return;
        }
        spriteRenderer.sprite = ingredientStats.gameSprite;

    }

    
}
