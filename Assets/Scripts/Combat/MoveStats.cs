using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStats : MonoBehaviour
{
    public SO_Move moveData;
    public SpriteRenderer sr;
    public int damage;
    public int healing;
    public int shielding; 
    public List<SO_Move.Debuff> debuffs;
    public float travelTime;

    public Vector3 travelTo = new Vector3(2,1,0);
    public Vector3 travelFrom = new Vector3(0,0,0);
    public float timeTravelled;
    void Start()
    {
        transform.position = Vector3.Lerp(travelTo, travelFrom, timeTravelled / travelTime);
        sr = GetComponent<SpriteRenderer>(); 
    }
     public void Setup(SO_Move moveData)
    {
        damage = moveData.damage;
        debuffs = moveData.debuffs;
        healing = moveData.healing;
        shielding = moveData.shielding;
        sr.sprite = moveData.sprite;
        travelTime = moveData.travelTime;

    }


    private void Update()
    {
        timeTravelled += Time.deltaTime;
        transform.position = Vector3.Lerp(travelTo, travelFrom, timeTravelled / travelTime);
        if (timeTravelled / travelTime < 1f) return;
        Destroy(gameObject);

    }
}
