using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Move")]
public class SO_Move : ScriptableObject
{
    public string moveName;
    public Sprite sprite;
    public enum Debuff { FROZEN, POISONED, BURNED, CONFUSED, BLIND};

    public int damage;
    public int healing;
    public int shielding;
    public float castTime;
    public float travelTime;
    public float channelTime;
    public List<Debuff> debuffs;
    public List<float> debuffDurations;
}
