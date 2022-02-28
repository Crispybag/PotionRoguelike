using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHazard : GridObject
{
    //[SerializeField] protected SO_OnGridManagerChanged onGridManager;
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

    /// <summary>
    /// Makes sure all debuff tiles are removed from board as soon as the time of the debuff runs out
    /// </summary>
    /// <param name="manager"> reference to the player manager for debuff timers </param>
    public void wearOut(PlayerManager manager)
    {
        foreach (SO_Move.Debuff debuff in manager.currentDebuffs)
        {
            if (debuff == debuffType) return;
        }

        if (onGridManager.OnRequestGridManager()) removeDebuffFromBoard();

    }

    /// <summary>
    /// setup all scriptable events
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        playerStats.onStatsChanged.AddListener(wearOut);
    }

    /// <summary>
    /// clear all scriptable events
    /// </summary>
    protected override void OnDisable()
    {
        base.OnDisable();
        playerStats.onStatsChanged.RemoveListener(wearOut);
    }

    private void removeDebuffFromBoard()
    {
        
        gridManager.RemoveObjectsFromBoard(gameObject);
        Destroy(gameObject);
    }




}
