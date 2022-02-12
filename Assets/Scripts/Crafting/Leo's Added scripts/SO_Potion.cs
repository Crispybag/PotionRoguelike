using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "CraftingSystem/Potion")]
public class SO_Potion : ScriptableObject
{
    public enum PotionEffect { DMG, SHIELD, HEAL};
    public PotionEffect potionEffect;
    public int[] tierValues = new int[5];
}
