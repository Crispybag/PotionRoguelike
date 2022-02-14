using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Manager/PlayerManager")]
public class SO_PlayerStats : ScriptableObject
{
    // Start is called before the first frame update

    public UnityEvent<PlayerManager> onStatsChanged;

   
    public void onPlayerStatsChanged(PlayerManager playerManager)
    {
        onStatsChanged.Invoke(playerManager);
    }
}
