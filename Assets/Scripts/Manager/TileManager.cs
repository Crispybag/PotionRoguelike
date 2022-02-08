using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> objectsOnBoard;
    public static TileManager tileManager;
    [SerializeField] private Tilemap _tilemap;

    private void Awake()
    {
        if (tileManager != null) { Destroy(this.gameObject); return; }
        tileManager = this;
    }
    void Start()
    {
        objectsOnBoard = new List<GameObject>();
    }


    public void AddObjectsOnBoard(GameObject obj)
    {
        objectsOnBoard.Add(obj);
    }

    public void RemoveObjectsFromBoard(GameObject obj)
    {
        objectsOnBoard.Remove(obj);
    }

    public Tilemap GetTilemap()
    { return _tilemap; }


    public List<GameObject> getObjectsOnBoard()
    {
        return objectsOnBoard;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
