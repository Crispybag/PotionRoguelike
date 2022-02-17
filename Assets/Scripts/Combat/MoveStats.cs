using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStats : MonoBehaviour
{


    [HideInInspector]public SO_Move moveData;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public int damage;
    [HideInInspector] public int healing;
    [HideInInspector] public int shielding;
    [HideInInspector] public List<SO_Move.Debuff> debuffs;
    [HideInInspector] public float travelTime;

    [HideInInspector] public float timeTravelled;

    private SO_EnemyMoveTriggerManager manager;
    public Vector3 travelFrom;
    public Vector3 travelTo;
    void Start()
    {
        if (travelTime == 0)
        {
            manager.MoveReachedEnd(this);
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.Lerp(travelTo, travelFrom, timeTravelled / travelTime);
        sr = GetComponent<SpriteRenderer>(); 
    }
     public void Setup(SO_Move moveData, SO_EnemyMoveTriggerManager pManager)
    {
        travelFrom = transform.position;
        travelTo = transform.position + new Vector3(0, -6, 0);
        damage = moveData.damage;
        debuffs = moveData.debuffs;
        healing = moveData.healing;
        shielding = moveData.shielding;
        sr.sprite = moveData.sprite;
        travelTime = moveData.travelTime;
        manager = pManager;
    }


    private void Update()
    {
        timeTravelled += Time.deltaTime;



        transform.position = Vector3.Lerp(travelFrom, travelTo, timeTravelled / travelTime);
        if (timeTravelled / travelTime < 1f) return;
        manager.MoveReachedEnd(this);
        Destroy(gameObject);

    }
}
