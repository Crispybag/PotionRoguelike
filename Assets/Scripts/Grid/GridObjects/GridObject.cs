using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridObject : MonoBehaviour
{
    protected GridManager gridManager;
    
    //value to make sure the grid object is added to the game board
    bool isAdded = false;
    //allows for other objects to enter the same space
    public bool isStackable = false;



    //function to do board calculations
    public static Vector3Int ToVector3Int(Vector3 inVec)
    {
        Vector3Int outVector = new Vector3Int(Mathf.RoundToInt(inVec.x), Mathf.RoundToInt(inVec.y), Mathf.RoundToInt(inVec.z));

        return outVector;
    }
     public static Vector3Int ToVector3Int(float x, float y, float z)
    {
        Vector3Int outVector = new Vector3Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y), Mathf.RoundToInt(z));
        return outVector;
    }




    private void Awake()
    {
        //Add Game Object to Board
        if (GridManager.mapManager != null && GridManager.mapManager.getObjectsOnBoard() != null)
        {
            gridManager = GridManager.mapManager;
            gridManager.AddObjectsOnBoard(gameObject);
            isAdded = true;
        }
    }

    protected virtual void Start()
    {
        //In Case the Awake fails for whatever reason
        if (!isAdded)
        {
            gridManager = GridManager.mapManager;
            gridManager.AddObjectsOnBoard(gameObject);
        }
    }
    
}
