using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    // Start is called before the first frame update
    bool isAdded = false;
    public static Vector3Int ToVector3Int(Vector3 inVec)
    {
        Vector3Int outVector = new Vector3Int(Mathf.RoundToInt(inVec.x), Mathf.RoundToInt(inVec.y), Mathf.RoundToInt(inVec.z));

        return outVector;
    }


    private void Awake()
    {
        if (MapManager.mapManager != null && MapManager.mapManager.getObjectsOnBoard() != null)
        {
            MapManager.mapManager.AddObjectsOnBoard(gameObject);
            isAdded = true;
        }
    }

    void Start()
    {
        if(!isAdded)MapManager.mapManager.AddObjectsOnBoard(gameObject);
    }
    
}
