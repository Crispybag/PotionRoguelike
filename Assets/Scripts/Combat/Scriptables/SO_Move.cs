using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Move")]
public class SO_Move : ScriptableObject
{
    public string name;
    public enum Debuff { SLOW, STUN };

    public int damage;
    public int healing;
    public int shielding;
    public float castTime;
    public float travelTime;
    public List<Debuff> debuffs;
}
