using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFireHazard : GridHazard
{
    [SerializeField] int damage = 1;
    private bool lateStart = true;
    protected override void Start()
    {
        base.Start();
    }

    public override void affectPlayer(ref int pHealth)
    {
        base.affectPlayer(ref pHealth);
        pHealth -= damage;
        if (onGridManager.OnRequestGridManager())
        {
            gridManager.SpawnGridObject(gameObject);
            gridManager.RemoveObjectsFromBoard(gameObject);
        }
        Destroy(gameObject);

    }
}
