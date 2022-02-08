using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TileManager.tileManager.AddObjectsOnBoard(this.gameObject);
    }
    
}
