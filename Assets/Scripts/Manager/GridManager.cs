using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> objectsOnBoard;
    public static GridManager mapManager;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _garbagePrefab;
    [SerializeField] private float _gameSpeed = 4;
    private void Awake()
    {
        if (mapManager != null) { Destroy(gameObject); return; }
        mapManager = this;
        objectsOnBoard = new List<GameObject>();
    }
    void Start()
    {
    }
    public GameObject GetGarbagePrefab()
    {
        return _garbagePrefab;
    }

    public void AddObjectsOnBoard(GameObject obj)
    {
        objectsOnBoard.Add(obj);
    }

    public float GetGameSpeed()
    {
        return _gameSpeed;
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
