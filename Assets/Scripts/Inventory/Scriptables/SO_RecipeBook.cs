using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(menuName = "Manager/RecipeBook")]
public class SO_RecipeBook : ScriptableObject
{
    public SO_Recipe[] inGameRecipes;

    /// <summary>
    /// Fill the recipe book with all the recipe scriptables in the Resources/Recipes folder
    /// </summary>
    public void fillGameRecipes()
    {
        //Load all objects to an object array
        Object[] a = Resources.LoadAll("Recipes", typeof(SO_Recipe));

        //convert the object array to an SO_Recipe Array
        inGameRecipes = System.Array.ConvertAll(a, item => item as SO_Recipe);

        //make sure it does not reset upon Unity reload
        EditorUtility.SetDirty(this);
    }
        


}
