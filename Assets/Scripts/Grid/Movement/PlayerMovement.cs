using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{

    [SerializeField] SO_OnPlayerMoved onPlayerMoved;
    [SerializeField] GridPlayer gridPlayer;

    // Start is called before the first frame update
    private Vector3 oldestWalkDir;
    
    /// <summary>
    /// Once a tile has been succesfully found, update all necessary values and check for step on hazardous material (and make sure that player doesnt fall)
    /// </summary>
    /// <param name="walkDir"></param>
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

    /// <summary>
    /// returns vector based on most recent input
    /// </summary>
    /// <param name="hori"></param>
    /// <param name="vert"></param>
    /// <returns></returns>
    private Vector3 GetRecentInputVector(float hori, float vert)
    {        
        //if x changed since last movement
        if ((int)_moveDir.x != hori)
        {
            return new Vector3(hori, 0, 0);

        }

        //if y changed since last movement
        else if ((int)_moveDir.y != vert) 
        { 
            return new Vector3(0, vert, 0); 
        }

        //if player somehow bruhs it up and press both at exact same frame
        else
        {
            Debug.LogError("Congratulations, you somehow fucked up the input, go vertical");
            return new Vector3(0, vert, 0);
        }
    }

    /// <summary>
    /// generate final walk direction based on input
    /// </summary>
    /// <returns></returns>
    private Vector3 generateWalkDir()
    {
        //if both keys are held
        if (Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") != 0)
        {

            //if player tapped this frame, get the newest walk direction
            if (CustomGetAxisDown())
            {

                return GetRecentInputVector(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }

            //if the player didnt, return oldest walk direction
            else
            {
                if (_moveDir != new Vector3(0, 0, 0)) return _moveDir;
                else return oldestWalkDir;
            }
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
            if (_lerpVal >= 1.0f)
            {
                CanMove(_moveDir);
            }
            else if (CustomGetAxisDown())
            {
                if (_lerpVal < _fastTapLimiter) return;
                snapAllToEnd();
                CanMove(_moveDir);

            }
        }
    }

    /// <summary>
    /// basically returns true if an arrow key or wasd got slammed
    /// </summary>
    /// <returns></returns>
    private bool CustomGetAxisDown()
    {
        //hard limiter to prevent extremely jittery turns
        return (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W));   
    }

    /// <summary>
    /// check if the player is about to step on a hazard
    /// </summary>
    /// <param name="obj">object player steps on</param>
    /// <returns></returns>
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
