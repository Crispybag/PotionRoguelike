using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SelectorMovement : MonoBehaviour
{
    [SerializeField] private SO_InventorySelector inventorySelector;
    [SerializeField] public Tilemap _map;

    public Vector3 mapPosition { get;  private set; }




    private bool customGetInputDown()
    {
        return (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W));
    }

    //move one space in a certain direction
    private void moveTile()
    {
        Vector3Int moveTowards = new Vector3Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"), 0);
        Vector3Int intPosition = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

        if (!customGetInputDown()) return;
        
        if (!checkIfOnTile(moveTowards + intPosition))        
        {
            //check for a distance further ahead
            moveTowards *= 2;
        
        }

        if(checkIfOnTile(moveTowards + intPosition))
        {
            transform.position += new Vector3(moveTowards.x, moveTowards.y, transform.position.z);
        }
    }

    private bool checkIfOnTile(Vector3Int pPos)
    {
        Vector3 mapOffset = _map.transform.position;
        Vector3Int intMapOffset = new Vector3Int((int)mapOffset.x, (int)mapOffset.y, (int)mapOffset.z);


        TileBase _base = _map.GetTile(pPos - intMapOffset);

        return _base != null;
    }
    private void Update()
    {
        moveTile();
        mapPosition = new Vector3(transform.position.x - _map.origin.x, (_map.size.y-1) - transform.position.y + _map.origin.y, 0 );

        //TEMPORARY!!! REMOVE LATER
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("The Cursor is at location: (" + mapPosition.x + ", " + mapPosition.y + ")");
            inventorySelector.OnCursorSelected(this);
        }

    }


}
