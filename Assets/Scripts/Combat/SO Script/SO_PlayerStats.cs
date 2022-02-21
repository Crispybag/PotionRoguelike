using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Manager/PlayerManager")]
public class SO_PlayerStats : ScriptableObject
{
    public UnityEvent<PlayerManager> onStatsChanged;
    public Vector3 location;
   
    public void onPlayerStatsChanged(PlayerManager playerManager)
    {
        onStatsChanged.Invoke(playerManager);
    }

}
