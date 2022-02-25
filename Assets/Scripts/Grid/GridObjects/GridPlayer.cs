using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// returns true if the mouse walks off the board
/// </summary>
public class GridPlayer : GridObject
{
    public bool checkForBoardFallOff(Vector3 walkDir)
    {

        //check if the mouse doesnt go off grid
        //Also Make sure the z-axis never plays a role
        Vector3Int intPos = ToVector3Int(transform.position.x, transform.position.y, 0);
        Vector3Int intDir = ToVector3Int(walkDir.x, walkDir.y, 0);
        Vector3Int goalTile = intPos + intDir;
        Vector3Int mapOffset = ToVector3Int(gridManager.GetTilemap().transform.position);

        return gridManager.GetTilemap().GetTile(goalTile - new Vector3Int(mapOffset.x, mapOffset.y, 0)) == null;
    }
}
