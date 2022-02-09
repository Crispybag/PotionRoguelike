using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Movement : MonoBehaviour
{
    private float _playerSpeed;
    //[SerializeField] protected Tilemap _map;
    //Lerp vectors
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    protected Vector3 _moveDir;
    private bool wantsToMove;


    public bool CanMove(Vector3 direction)
    {
        //where I want to go
        Vector3Int goalTile = GridObject.ToVector3Int(transform.position) + GridObject.ToVector3Int(direction);
        foreach (GameObject obj in MapManager.mapManager.getObjectsOnBoard())
        {
            //check if there is an object on the location
            if (GridObject.ToVector3Int(obj.transform.position) == goalTile)
            {

                //if it doesnt have a movement script
                if (!obj.GetComponent<Movement>()) return false;

                wantsToMove = obj.GetComponent<Movement>().CanMove(direction);
                if (!wantsToMove) { return false; }
                else
                {
                    _moveDir = direction;
                    updateLerp(direction);
                    return true;
                }
            }
        }

        wantsToMove = true;
        _moveDir = direction;
        updateLerp(direction);
        return true;
    }

    //determines how far the player is in the lerp movement
    protected float _lerpVal;
    
    void Start()
    {
        _lerpVal = 1.0f;
        _startPosition = transform.position;
        _endPosition = transform.position;
        _playerSpeed = MapManager.mapManager.GetGameSpeed();
    }

    //function to update the lerp once it is done
    protected virtual void updateLerp(Vector3 walkDir)
    {

            int hori = Mathf.RoundToInt(walkDir.x);
            int vert = Mathf.RoundToInt(walkDir.y);
            _startPosition = transform.position;
            _endPosition = transform.position + new Vector3(hori, vert, 0);
            _lerpVal = 0f;

            wantsToMove = false;
            _moveDir = new Vector3(0, 0, 0);
        
    }


    public Vector3Int GetEndPos()
    {
        return GridObject.ToVector3Int(_endPosition);
    }



    // Update is called once per frame
    protected virtual void Update()
    {
        //update lerp
        _lerpVal += _playerSpeed * Time.deltaTime + 2 *_lerpVal * _playerSpeed * Time.deltaTime;
        
        //lerp the player between the 2 coordinates
        transform.position = Vector3.Lerp(_startPosition, _endPosition, _lerpVal);
    }

    private void OnDestroy()
    {
        MapManager.mapManager.RemoveObjectsFromBoard(this.gameObject);
    }
}
