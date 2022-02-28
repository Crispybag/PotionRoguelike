using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFireHazard : GridHazard
{
    [SerializeField] int damage = 1;
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// When Player steps on tile, deal damage and reposition
    /// </summary>
    /// <param name="pHealth"> player health </param>
    public override void affectPlayer(ref int pHealth)
    {
        base.affectPlayer(ref pHealth);
        //deal damage here
        pHealth -= damage;

        //remove firehazard from grid and respawn it
        if (onGridManager.OnRequestGridManager())
        {
            gridManager.SpawnGridObject(gameObject);
            gridManager.RemoveObjectsFromBoard(gameObject);
        }
        Destroy(gameObject);

    }
}
