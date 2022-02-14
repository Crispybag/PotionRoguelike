using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Manager/GetCurrentRecipe")]
public class SO_GetCurrentRecipe : ScriptableObject
{
    // Start is called before the first frame update

    public UnityEvent<SO_Recipe> onRecipeChanged;


    public void RecipeChanged(SO_Recipe recipe)
    {
        onRecipeChanged.Invoke(recipe);
    }
}
