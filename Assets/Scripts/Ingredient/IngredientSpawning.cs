using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class IngredientSpawning : MonoBehaviour
{

    private Tilemap _map;
    private int amountOfGarbage;
    private void Start()
    {
        amountOfGarbage = 2;
        _map = MapManager.mapManager.GetTilemap();
    }

    //respawn ingredient at new place
    private void RespawnIngredient(GameObject gObject)
    {
        Vector3Int mapOffset = new Vector3Int(Mathf.RoundToInt(_map.transform.position.x), Mathf.RoundToInt(_map.transform.position.y));
        Vector3Int respawnPos = new Vector3Int(0,0,0);

        //make sure it found a proper place to spawn
        List<Vector3Int> possibleSpawnCoords = new List<Vector3Int>();
        for (int x = 0; x < _map.size.x; x++)
        {
            //dont spawn on top row (-1)
            for (int y = 0; y < _map.size.y - 1; y++)
            {
                bool isAvailable = false;
                Vector3Int position =  GridObject.ToVector3Int(new Vector3(x + _map.origin.x, y + _map.origin.y, 0));
                //check if any objects are on this position
                foreach (GameObject obj in MapManager.mapManager.getObjectsOnBoard())
                {
                    isAvailable = true;
                    //return false if spot isnt available
                    if (GridObject.ToVector3Int(obj.transform.position) == position)
                    {
                        isAvailable = false;
                        break;
                    }
                    if (!obj.GetComponent<Movement>()) break;

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

        //dont instantiate if there is nothing to pick from
        if (possibleSpawnCoords.Count == 0) return;

        //get possible spawn coordinates
        respawnPos = possibleSpawnCoords[Random.Range(0, possibleSpawnCoords.Count)];

        //instantiate it
        Instantiate(gObject, new Vector3(respawnPos.x, respawnPos.y, transform.position.z), gameObject.transform.rotation);
    }


    public void OnGameObjectDestroy()
    {
        RespawnIngredient(gameObject);
        for (int i = 0; i < amountOfGarbage; i++)
        {
            RespawnIngredient(MapManager.mapManager.GetGarbagePrefab());
        }
    }
}
