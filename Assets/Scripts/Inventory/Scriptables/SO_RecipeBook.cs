using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Manager/RecipeBook")]
public class SO_RecipeBook : ScriptableObject
{
    public SO_Recipe[] inGameRecipes;

    public void fillGameRecipes()
    {
        Object[] a = Resources.LoadAll("Recipes", typeof(SO_Recipe));
        inGameRecipes = System.Array.ConvertAll(a, item => item as SO_Recipe);
    }
        


}
