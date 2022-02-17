using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITierScript : MonoBehaviour
{
    [SerializeField] private SO_GetCurrentRecipe getCurrentRecipe;
    [SerializeField] private Text potionType;
    [SerializeField] private Text textCombo0;
    [SerializeField] private Text textCombo1;
    [SerializeField] private Text textCombo2;
    [SerializeField] private Text textCombo3;
    [SerializeField] private Text textCombo4;


    private void OnEnable()
    {
        getCurrentRecipe.onRecipeChanged.AddListener(UpdateUI);
    }

    private void OnDisable()
    {
        getCurrentRecipe.onRecipeChanged.RemoveListener(UpdateUI);
    }


    private void UpdateUI(SO_Recipe currentRecipe)
    {
        if (currentRecipe == null)
        {
            potionType.text = "-";
            textCombo0.text = "-";
            textCombo1.text = "-";
            textCombo2.text = "-";
            textCombo3.text = "-";
            textCombo4.text = "-";
            return;
        }


        SO_Potion potion = currentRecipe.potion;
        potionType.text = potion.potionEffect.ToString();
        textCombo0.text = potion.tierValues[0].ToString();
        textCombo1.text = potion.tierValues[1].ToString();
        textCombo2.text = potion.tierValues[2].ToString();
        textCombo3.text = potion.tierValues[3].ToString();
        textCombo4.text = potion.tierValues[4].ToString();
    }
}
