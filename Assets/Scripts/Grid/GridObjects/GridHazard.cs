using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHazard : GridObject
{
    [SerializeField] SO_OnPlayerSteppedOnHazard hazardManager;
    [SerializeField] SO_PlayerStats playerStats;
    
    [SerializeField] SO_Move.Debuff debuffType;
    
    
    
    public void PlayerSteppedOnHazard()
    {
        hazardManager.OnPlayerSteppedOnHazard(this);
    }

    public virtual void affectPlayer(ref int pHealth)
    {
        //override function for other hazards to affect the player here somehow
        //actually might not be an insanely bright idea as the player has to take damage on its own so might scrap this
        //but it would be nice if I could write what the tiles do in their respective classes, so might still use this later
    }

    public void wearOut(PlayerManager manager)
    {
        foreach (SO_Move.Debuff debuff in manager.currentDebuffs)
        {
            if (debuff == debuffType) return;
        }
        GridManager.mapManager.RemoveObjectsFromBoard(gameObject);
        Destroy(gameObject);
    }

    public void OnEnable()
    {
        playerStats.onStatsChanged.AddListener(wearOut);
    }

    public void OnDisable()
    {
        playerStats.onStatsChanged.RemoveListener(wearOut);
    }

}
