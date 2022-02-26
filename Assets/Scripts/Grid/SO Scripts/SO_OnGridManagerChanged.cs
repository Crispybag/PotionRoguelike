using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Manager/Get Grid Manager")]
public class SO_OnGridManagerChanged : ScriptableObject
{
    public UnityEvent<GridManager> onGridManagerChanged;
    public UnityEvent<bool> requestGridManager;
    public bool foundGridManager;
    public void OnGridManagerChanged(GridManager gridManager)
    {
        onGridManagerChanged.Invoke(gridManager);
        foundGridManager = false;
    }

    public bool OnRequestGridManager()
    {
        requestGridManager.Invoke(true);

        if (!foundGridManager) return foundGridManager;
        
        foundGridManager = false;
        return true;
        
    }



}
