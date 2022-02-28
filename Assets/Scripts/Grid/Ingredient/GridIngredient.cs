using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GridIngredient : GridObject
{
    private Tilemap _map;

    protected override void Start()
    {
        base.Start();
        _map = gridManager.GetTilemap();
    }
    //make sure objects get deleted once they fall off the map
    
    public bool checkForFallAndRemove(Vector3 pushDirection)
    {
        Vector3Int iPushDirection = ToVector3Int(pushDirection);

        //vectors to find edges
        Vector3Int mapOffset = new Vector3Int(Mathf.RoundToInt(_map.transform.position.x), Mathf.RoundToInt(_map.transform.position.y));
        Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));

        //destroy if it cant find a tile below it
        if (!gridManager.GetTilemap().GetTile(intPos - mapOffset + iPushDirection))
        {
            //make ingredientspawning do its thing if its there
            if (gameObject.GetComponent<IngredientSpawning>()) gameObject.GetComponent<IngredientSpawning>().OnGameObjectDestroy();
            return true;
        }

        return false;
    }

    public void DestroyIngredient(Vector3 pushDirection)
    {
        if (pushDirection.x == 0 && pushDirection.y < 0)
        {
            // go to crafting recipe (change later if able)
            FindObjectOfType<CraftingManager>().AddIngredient(gameObject);
        }

        Destroy(this.gameObject);

    }

}
