using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Combat/Phase")]
public class SO_EnemyPhase :ScriptableObject
{
    public enum RequirementType { HEALTH, TIME};

    //determine whether this phase triggers due to health or time
    public RequirementType requirementType;
    //randomize what combos it gets
    [Range(0f, 1f)]public float healthRequirement;
    public float timeRequirement;
    public List<SO_EnemyCombo> enemyCombos;
    public bool isStartingPhase;

    /// <summary>
    /// start a combo from current phase
    /// </summary>
    public void StartCombo(ref SO_EnemyCombo currentEnemyCombo, ref bool lastShotFired)
    {
        //always pick at random
        currentEnemyCombo = enemyCombos[Random.Range(0, enemyCombos.Count)];
        lastShotFired = false;
        Debug.Log("Do Combo! " + currentEnemyCombo.name);
    }


}
