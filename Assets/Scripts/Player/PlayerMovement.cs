using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    // Start is called before the first frame update
    private Vector3 oldestWalkDir;

    protected override void updateLerp(Vector3 walkDir)
    {
        float hori = walkDir.x;
        float vert = walkDir.y;

        //check if the mouse doesnt go off grid
        Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        Vector3Int intDir = new Vector3Int(Mathf.RoundToInt(hori), Mathf.RoundToInt(vert), 0);
        Vector3Int goalTile = intPos + intDir;
        Vector3Int mapOffset = new Vector3Int(Mathf.RoundToInt(TileManager.tileManager.GetTilemap().transform.position.x), Mathf.RoundToInt(TileManager.tileManager.GetTilemap().transform.position.y));

        if (TileManager.tileManager.GetTilemap().GetTile(goalTile - mapOffset))
        {
            oldestWalkDir = new Vector3(hori, vert, 0);
            base.updateLerp(walkDir);
        }
        
    }

    //System to make player move only horizontally or vertically when both movements are put in
    //it prioritizes new movement that it wasnt going into
    private Vector3 accountForDiagonal(float hori, float vert)
    {
        //was moving vertically
        if (oldestWalkDir.x == 0)
        {
            return new Vector3(hori, 0, 0);

        }
        else if (oldestWalkDir.y == 0) { return new Vector3(0, vert, 0); }
        else
        {
            Debug.LogError("oldestWalkDir is diagonal");
            return new Vector3(hori, vert, 0);
        }
    }

    //determine what the resulting walk direction should be
    private Vector3 generateWalkDir()
    {
        //if both keys are held
        if (Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") != 0)
        {
            return accountForDiagonal(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        //if one key is held
        else
        {
            return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }

    protected override void Update()
    {
        base.Update();


        Vector3 futureWalkDir = new Vector3(0, 0, 0);


        //break loop if player doesnt have any inputs
        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0) return;

        //determine which direction the mouse will walk in based on input
        futureWalkDir = generateWalkDir();
        _moveDir = futureWalkDir;
        //accept new input when the lerp is finished
        if (_lerpVal >= 1.0f) CanMove(_moveDir);
    }
}
