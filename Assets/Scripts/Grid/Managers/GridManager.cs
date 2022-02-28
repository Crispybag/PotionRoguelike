using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SO_OnGridManagerChanged onGridManagerChanged;
    [SerializeField] SO_PlayerStats onPlayerStatsChanged;
    private List<GameObject> _objectsOnBoard;
    //public static GridManager gridManager;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _garbagePrefab;
    [SerializeField] private float _gameSpeed = 4;
    [SerializeField] public float jitteriness = 6;
    [SerializeField]
    [Range(0f, 1f)]
    public float _fastTapLimiter = 0.5f;
    [SerializeField] [Range(0f,1f)]float slowStrength = 0.5f;
    private bool isFrozen;
    private Tilemap _map;
    private void Awake()
    {
        _objectsOnBoard = new List<GameObject>();
        _map = GetTilemap();
    }

    public GameObject GetGarbagePrefab()
    {
        return _garbagePrefab;
    }

    public void AddObjectsOnBoard(GameObject obj)
    {
        _objectsOnBoard.Add(obj);
    }

    public float GetGameSpeed()
    {
        return _gameSpeed;
    }

    public void RemoveObjectsFromBoard(GameObject obj)
    {
        _objectsOnBoard.Remove(obj);
    }

    public Tilemap GetTilemap()
    { return _tilemap; }


    public List<GameObject> getObjectsOnBoard()
    {
        return _objectsOnBoard;
    }

    private void refreshList()
    {
        foreach(GameObject obj in _objectsOnBoard)
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
        for (int x = 0; x < _map.size.x; x++)
        {
            //dont spawn on top row (-1)
            for (int y = 0; y < _map.size.y; y++)
            {
                bool isAvailable = true;
                Vector3Int position = GridObject.ToVector3Int(new Vector3(x + _map.origin.x, y + _map.origin.y, 0));
                //break loop immeadiately if no tile is present on the coordinates
                if(_map.GetTile(position) == null || _map.GetTile(new Vector3Int(position.x, position.y + 1, position.z)) == null)
                {
                    isAvailable = false;
                    continue;
                }
                //check if any objects are on this position
                foreach (GameObject obj in getObjectsOnBoard())
                {
                    if (obj.GetComponent<GridObject>().isStackable) continue;
                    
                        //return false if spot isnt available
                    if (GridObject.ToVector3Int(obj.transform.position) == position)
                    {
                        isAvailable = false;
                        break;
                    }
                    if (!obj.GetComponent<Movement>())
                    {

                        isAvailable = false;
                        break;
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
        if (_map == null) _map = GetTilemap();
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

    //make a decoupled nonsingletone way of returning the grid manager
    private void OnEnable()
    {
        onGridManagerChanged.requestGridManager.AddListener(onGridManagerRequest);
        onPlayerStatsChanged.onStatsChanged.AddListener(changePlayerSpeed);
    }
    private void OnDisable()
    {
        onGridManagerChanged.requestGridManager.RemoveListener(onGridManagerRequest);
        onPlayerStatsChanged.onStatsChanged.RemoveListener(changePlayerSpeed);

    }


    private void OnValidate()
    {
        onGridManagerChanged.OnGridManagerChanged(this);
    }

    //this invokes functions only if a grid manager actually exists, so it doesnt do functions when it doesnt have a response call
    //I do need to figure out a way in case it can't find anything
    private void onGridManagerRequest(bool e)
    {
        onGridManagerChanged.OnGridManagerChanged(this);
        onGridManagerChanged.foundGridManager = true;
    }

    private void changePlayerSpeed(PlayerManager playerManager)
    {
        if (!isFrozen)
        {
            foreach(SO_Move.Debuff debuff in playerManager.currentDebuffs)
            {
                if(debuff == SO_Move.Debuff.FROZEN)
                {
                    isFrozen = true;
                    _gameSpeed *= 1 - slowStrength;
                    _fastTapLimiter++;
                    onGridManagerChanged.OnGridManagerChanged(this);


                    return;
                }
            }

        }
        else
        {
            bool stillFrozen = false;
            foreach (SO_Move.Debuff debuff in playerManager.currentDebuffs)
            {
                if (debuff == SO_Move.Debuff.FROZEN) 
                {
                    stillFrozen = true;
                    break;
                }
            }
            if (stillFrozen) return;
            _gameSpeed /= 1 - slowStrength;
            _fastTapLimiter--;
            onGridManagerChanged.OnGridManagerChanged(this);

            isFrozen = false;
        }
    }

    


    // Update is called once per frame
    void Update()
    {
        refreshList();
    }
}
