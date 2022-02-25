using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Manager/OnPlayerSteppedOnHazard")]
public class SO_OnPlayerSteppedOnHazard : ScriptableObject
{
    public UnityEvent<GridHazard> onPlayerSteppedOnHazard;
    
    public void OnPlayerSteppedOnHazard(GridHazard hazard)
    {
        onPlayerSteppedOnHazard.Invoke(hazard);
    }





}

