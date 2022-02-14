using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionStats : MonoBehaviour
{
    public SO_Potion.PotionEffect potionEffect;
    public int strength;

    public void Setup(SO_Potion potion, CraftingManager manager)
    {
        strength = potion.tierValues[manager.recipeTier];
        potionEffect = potion.potionEffect;
        manager.moveManager.MoveReachedEnd(this);
    }
    private void Update()
    {
    Destroy(gameObject);    
    }


}
