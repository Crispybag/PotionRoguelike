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
    [SerializeField] public float jitteriness = 6;


    private Tilemap _map;


    private void Awake()
    {
        if (mapManager != null) { Destroy(mapManager.gameObject);}
        mapManager = this;
        objectsOnBoard = new List<GameObject>();
        _map = GetTilemap();
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

    private void refreshList()
    {
        foreach(GameObject obj in objectsOnBoard)
        {
            if (obj == null)
            {
                RemoveObjectsFromBoard(obj);
                break;
            }
        }
    }


    private List<Vector3Int> GenerateAvailablePlaces()
    {
        List<Vector3Int> possibleSpawnCoords = new List<Vector3Int>();
        for (int x = 0; x < _map.size.x - 1; x++)
        {
            //dont spawn on top row (-1)
            for (int y = 0; y < _map.size.y - 1; y++)
            {
                bool isAvailable = true;
                Vector3Int position = GridObject.ToVector3Int(new Vector3(x + _map.origin.x, y + _map.origin.y, 0));
                //check if any objects are on this position
                foreach (GameObject obj in GridManager.mapManager.getObjectsOnBoard())
                {
                    //return false if spot isnt available
                    if (GridObject.ToVector3Int(obj.transform.position) == position)
                    {
                        isAvailable = false;
                        break;
                    }
                    if (!obj.GetComponent<Movement>())
                    {
                        if (!obj.GetComponent<GridObject>().isStackable)
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    //also make sure it doesnt spawn on a tile where an ingredient is currently moving towards
                    else if (obj.GetComponent<Movement>().GetEndPos() == position)
                    {
                        isAvailable = false;
                        break;
                    }
                }
                if (isAvailable) possibleSpawnCoords.Add(position);
            }
        }
        return possibleSpawnCoords;
    }

    //respawn ingredient at new place
    public void SpawnGridObject(GameObject gObject)
    {
        if (_map == null) _map = mapManager.GetTilemap();
        //make sure it found a proper place to spawn

        List<Vector3Int> possibleSpawnCoords = GenerateAvailablePlaces();
        //dont instantiate if there is nothing to pick from
        if (possibleSpawnCoords.Count == 0) return;

        //get possible spawn coordinates
        Vector3Int respawnPos = possibleSpawnCoords[Random.Range(0, possibleSpawnCoords.Count)];

        //instantiate it
        GameObject newIngredient = Instantiate(gObject, new Vector3(respawnPos.x, respawnPos.y, transform.position.z), gameObject.transform.rotation);
        newIngredient.name = gObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        refreshList();
    }
}
