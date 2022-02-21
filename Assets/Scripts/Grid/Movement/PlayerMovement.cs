using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    // Start is called before the first frame update
    private Vector3 oldestWalkDir;
    [SerializeField]
    [Range(0f, 1f)]
    float _fastTapLimiter = 0.5f;
    protected override void updateLerp(Vector3 walkDir)
    {
        float hori = walkDir.x;
        float vert = walkDir.y;

        //check if the mouse doesnt go off grid
        Vector3Int intPos = GridObject.ToVector3Int(transform.position);
        Vector3Int intDir = new Vector3Int(Mathf.RoundToInt(hori), Mathf.RoundToInt(vert), 0);
        Vector3Int goalTile = intPos + intDir;
        Vector3Int mapOffset = GridObject.ToVector3Int(GridManager.mapManager.GetTilemap().transform.position);

        if (GridManager.mapManager.GetTilemap().GetTile(goalTile - mapOffset))
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


        //break loop if player doesnt have any inputs
        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {

            //determine which direction the mouse will walk in based on input

            Vector3 futureWalkDir = generateWalkDir();
            _moveDir = futureWalkDir;
            //accept new input when the lerp is finished
            if (_lerpVal >= 1.0f) CanMove(_moveDir);
            else if (CustomGetAxisDown())
            {
               snapAllToEnd();
               CanMove(_moveDir);
                
            }
        }
    }

    private bool CustomGetAxisDown()
    {
        //hard limiter to prevent extremely jittery turns
        if (_lerpVal < _fastTapLimiter) return false;
        return (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W));   
    }





}
