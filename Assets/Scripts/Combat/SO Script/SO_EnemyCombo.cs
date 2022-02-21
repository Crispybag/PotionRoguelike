using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Combat/Combo")]
public class SO_EnemyCombo : ScriptableObject
{
    //always fire these attacks in order
    public string comboName;
    public List<SO_Move> moves;



    public void RegisterShot(ref int currentMove, ref bool lastShotFired)
    {
        currentMove++;
        //fire shot currentmove is pointing at
        if (currentMove >= (moves.Count)) { lastShotFired = true; currentMove = 0; }
        else
        {
            lastShotFired = false;
        }
    }



}
