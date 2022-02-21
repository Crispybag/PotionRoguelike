using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryManager))]
public class E_FillRecipe : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InventoryManager i = (InventoryManager)target;
        if(GUILayout.Button("Fill Recipe Book"))
        {
            i.FillRecipeBook();
        }
    }
}
