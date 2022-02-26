using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoisonHazard : GridHazard
{
    [SerializeField] SO_OnPlayerMoved onPlayerMoved;
    [SerializeField] int damage = 1;
    [SerializeField] float lifeTime = 1;
    private bool timeBased = true;
    public override void affectPlayer(ref int pHealth)
    {
        base.affectPlayer(ref pHealth);
        pHealth -= damage;
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        onPlayerMoved.onPlayerMoved.AddListener(spawnToxicTile);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        onPlayerMoved.onPlayerMoved.RemoveListener(spawnToxicTile);
    }


    void spawnToxicTile(PlayerMovement playerMovement)
    {
        if (timeBased) return;
        Instantiate(gameObject, playerMovement.startPosition, gameObject.transform.rotation);
        reduceLifeTime(1);     
    }

    private void reduceLifeTime(float pTime)
    {
        lifeTime -= pTime;
        if (lifeTime <= 0)
        {
            gridManager.RemoveObjectsFromBoard(gameObject);
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (!timeBased) return;
        reduceLifeTime(Time.deltaTime);

    }

}
