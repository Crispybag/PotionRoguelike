using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : Movement
{

    private void fallAndRemove()
    {
        Vector3Int mapOffset = new Vector3Int(Mathf.RoundToInt(TileManager.tileManager.GetTilemap().transform.position.x), Mathf.RoundToInt(TileManager.tileManager.GetTilemap().transform.position.y));
        Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        if (! TileManager.tileManager.GetTilemap().GetTile(intPos - mapOffset))
        {
            Destroy(this.gameObject);
        }
    }
    protected override void Update()
    {
        base.Update();
        fallAndRemove();
    }
}
