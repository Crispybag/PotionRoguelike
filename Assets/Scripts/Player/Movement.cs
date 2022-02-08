using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _playerSpeed;
    [SerializeField] Tilemap _map;
    //Lerp vectors
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    //determines how far the player is in the lerp movement
    private float _lerpVal;
    private Vector3 oldestWalkDir;




    void Start()
    {
        _lerpVal = 1.0f;
        _startPosition = transform.position;
        _endPosition = transform.position;
    }

    //function to update the lerp once it is done
    private void updateLerp(Vector3 walkDir)
    {
        float hori = walkDir.x;
        float vert = walkDir.y;

        //check if the mouse doesnt go off grid
        Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        Vector3Int intDir = new Vector3Int(Mathf.RoundToInt(hori), Mathf.RoundToInt(vert), 0);
        Vector3Int goalTile = intPos + intDir;
        Vector3Int mapOffset = new Vector3Int(Mathf.RoundToInt(_map.transform.position.x), Mathf.RoundToInt(_map.transform.position.y));

        if (_map.GetTile(goalTile - mapOffset ))
        {
            _startPosition = transform.position;
            _endPosition = transform.position + new Vector3(hori, vert, 0);
            _lerpVal = 0f;
            oldestWalkDir = new Vector3(hori, vert, 0);
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


    // Update is called once per frame
    void Update()
    {
        //update lerp
        _lerpVal += _playerSpeed * Time.deltaTime;
        
        //lerp the player between the 2 coordinates
        transform.position = Vector3.Lerp(_startPosition, _endPosition, _lerpVal);


        Vector3 futureWalkDir = new Vector3(0,0,0);


        //break loop if player doesnt have any inputs
        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0) return;

        //determine which direction the mouse will walk in based on input
        futureWalkDir = generateWalkDir();
        
        //accept new input when the lerp is finished
        if (_lerpVal >= 1.0f) updateLerp(futureWalkDir);
    }
}
