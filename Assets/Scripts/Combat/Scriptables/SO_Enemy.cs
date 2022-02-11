using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Combat/Enemy")]
public class SO_Enemy : ScriptableObject
{
    public string enemyName;
    public Sprite sprite;
    public int baseHealth;
    public float craftingSpeed; //multiply this with crafting time of a potion. could be fun to have enemies be able to craft the same potion faster
    public List<SO_EnemyPhase> enemyPhases;


    public void GoToNextPhase(ref bool isLastShotFired, ref SO_EnemyCombo currentCombo, ref SO_EnemyPhase currentEnemyPhase, int hp, float time)
    {
        float percentageHealth = (float)hp / (float)baseHealth;
        bool changedPhase = false;
        foreach (SO_EnemyPhase phase in enemyPhases)
        {

            //prioritize time based phase requirement
            if(phase.requirementType == SO_EnemyPhase.RequirementType.TIME)
            {
                //break if the time is still below time requirement
                if (time <= phase.timeRequirement) continue;

                //dont get a time requirement phase with lower time requirement
                if (phase.timeRequirement <= currentEnemyPhase.timeRequirement) continue;

                //set currentEnemyPhase to value with highest time requirement
                Debug.Log("Go to phase: " + phase.name);
                currentEnemyPhase = phase;
                changedPhase = true;
               
            }

            //cant do health requirements after a time requirement has been selected
            if (currentEnemyPhase.requirementType == SO_EnemyPhase.RequirementType.TIME) continue;

            //needs to be equal or lower than current hp
            if (phase.healthRequirement >= percentageHealth) continue;

            //dont replace when the health requirement is lower
            if (phase.healthRequirement <= currentEnemyPhase.healthRequirement) continue;

            //set current enemy phase to value with highest hp requirement
            Debug.Log("Go to phase: " + phase.name);
            currentEnemyPhase = phase;
            changedPhase = true;

        }
        if (!changedPhase) return;
        
        
        
        currentEnemyPhase.StartCombo(ref currentCombo, ref isLastShotFired);

    }

}
