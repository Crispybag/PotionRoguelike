using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected SO_OnGridManagerChanged onGridManager;
    protected GridManager _gridManager;   


    private float _playerSpeed;
    private float _jitteriness;
    
    //[SerializeField] protected Tilemap _map;
    //Lerp vectors
    public Vector3 startPosition;
    protected Vector3 _endPosition;

    protected Vector3 _moveDir;
    private bool wantsToMove;
    protected float _lerpVal;

    float _timeValue = 0f;
    protected float _fastTapLimiter = 0.5f;

    public bool CanMove(Vector3 direction)
    {
        if (!onGridManager.OnRequestGridManager()) return false;

        //where I want to go
        Vector3Int goalTile = GridObject.ToVector3Int(transform.position) + GridObject.ToVector3Int(direction);
        foreach (GameObject obj in _gridManager.getObjectsOnBoard())
        {
            if (obj == null) continue;
            //check if there is an object on the location
            if (GridObject.ToVector3Int(obj.transform.position) == goalTile)
            {

                GridObject gObj = null;
                if (obj.GetComponent<GridObject>()) gObj = obj.GetComponent<GridObject>();

                //when it doesnt contain a gridobject script
                else
                {
                    continue;
                }

                //also allow it to move when the gameobject is stackable, no movement needed
                if(gObj.isStackable)
                {
                    continue;
                }

                //if it doesnt have a movement script
                if (!obj.GetComponent<Movement>()) return false;
                wantsToMove = obj.GetComponent<Movement>().CanMove(direction);


               
                //when it does contain a grid object and the object wont budge nor is it stackable
                if (!wantsToMove) 
                { 
                    return false; 
                }
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
    
    void Start()
    {
        if (!onGridManager.OnRequestGridManager()) return;
        _lerpVal = 1.0f;
        startPosition = transform.position;
        _endPosition = transform.position;
        _playerSpeed = _gridManager.GetGameSpeed();
        _jitteriness = _gridManager.jitteriness;
        _fastTapLimiter = _gridManager._fastTapLimiter;

    }

    //function to update the lerp once it is done
    protected virtual void updateLerp(Vector3 walkDir)
    {

            int hori = Mathf.RoundToInt(walkDir.x);
            int vert = Mathf.RoundToInt(walkDir.y);
            startPosition = transform.position;
            _endPosition = transform.position + new Vector3(hori, vert, 0);
            _lerpVal = 0f;

            wantsToMove = false;
            _moveDir = new Vector3(0, 0, 0);
        
    }


    public Vector3Int GetEndPos()
    {
        return GridObject.ToVector3Int(_endPosition);
    }

    protected void snapAllToEnd()
    {
        if (!onGridManager.OnRequestGridManager()) return;

        foreach (GameObject obj in _gridManager.getObjectsOnBoard())
        {
            if (!obj.GetComponent<Movement>()) continue;
            Movement m = obj.GetComponent<Movement>();
            m._lerpVal = 1f;
            obj.transform.position = Vector3.Lerp(m.startPosition, m._endPosition, m._lerpVal);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        _timeValue += Time.deltaTime ;
        if(_timeValue > 0.1f) { _timeValue = 0f;}
        //update lerp
        _lerpVal += _playerSpeed * Time.deltaTime + _jitteriness * (1-_lerpVal) * _playerSpeed * Time.deltaTime;
        
        //lerp the player between the 2 coordinates
        transform.position = Vector3.Lerp(startPosition, _endPosition, _lerpVal);
    }

    private void OnDisable()
    {

        if(onGridManager.OnRequestGridManager())  _gridManager.RemoveObjectsFromBoard(gameObject);
        onGridManager.onGridManagerChanged.RemoveListener(getGridManager);
    }

    private void OnEnable()
    {
        onGridManager.onGridManagerChanged.AddListener(getGridManager);
    }


    private void getGridManager(GridManager gridManager)
    {
        _gridManager = gridManager;
        _playerSpeed = _gridManager.GetGameSpeed();
        _jitteriness = _gridManager.jitteriness;
        _fastTapLimiter = _gridManager._fastTapLimiter;
    }


}
