using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TempEnemy")]
public class TempEnemy : ScriptableObject
{
    public string title;
    public Sprite icon;
    [Range(0,100)]
    public int winChance = 50;
}
