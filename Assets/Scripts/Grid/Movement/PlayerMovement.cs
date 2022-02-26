using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{

    [SerializeField] SO_OnPlayerMoved onPlayerMoved;
    [SerializeField] GridPlayer gridPlayer;

    // Start is called before the first frame update
    private Vector3 oldestWalkDir;
    [SerializeField]
    [Range(0f, 1f)]
    float _fastTapLimiter = 0.5f;
    protected override void updateLerp(Vector3 walkDir)
    {
        if (!onGridManager.OnRequestGridManager()) return;

        if (!gridPlayer.checkForBoardFallOff(walkDir))
        {
            foreach (GameObject obj in _gridManager.getObjectsOnBoard())
            {
                if (GridObject.ToVector3Int(obj.transform.position) != GridObject.ToVector3Int(transform.position + walkDir)) continue;
                if (checkForHazardousTerrain(obj)) break;
                
            }
            oldestWalkDir = new Vector3(walkDir.x, walkDir.y, 0);
            base.updateLerp(walkDir);
            onPlayerMoved.OnPlayerMoved(this);
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

    private bool checkForHazardousTerrain(GameObject obj)
    {
        if (!obj.GetComponent<GridObject>()) return false;
        GridObject gObj = obj.GetComponent<GridObject>();

        if (!gObj.isStackable) return false;
        if(gObj is GridHazard)
        {
            GridHazard hazard = gObj as GridHazard;
            hazard.PlayerSteppedOnHazard();
            return true;
        }
        return false;


    }



}
