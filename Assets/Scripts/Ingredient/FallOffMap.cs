using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class FallOffMap : MonoBehaviour
{
    private Tilemap _map;

    private void Start()
    {
        _map = MapManager.mapManager.GetTilemap();
    }
    //make sure objects get deleted once they fall off the map
    private void fallAndRemove()
    {
        //vectors to find edges
        Vector3Int mapOffset = new Vector3Int(Mathf.RoundToInt(_map.transform.position.x), Mathf.RoundToInt(_map.transform.position.y));
        Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));

        //destroy if it cant find a tile below it
        if (!MapManager.mapManager.GetTilemap().GetTile(intPos - mapOffset))
        {
            //make ingredientspawning do its thing if its there
            if (gameObject.GetComponent<IngredientSpawning>()) gameObject.GetComponent<IngredientSpawning>().OnGameObjectDestroy();
            Destroy(this.gameObject);
        }
    }
    void Update()
    {
        fallAndRemove();
    }
}
