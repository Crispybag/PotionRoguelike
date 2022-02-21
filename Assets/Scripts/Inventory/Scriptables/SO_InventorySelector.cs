using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Manager/Inventory Selector")]
public class SO_InventorySelector : ScriptableObject
{
    public UnityEvent<SelectorMovement> onCursorSelected;

    public void OnCursorSelected(SelectorMovement pMove)
    {
        onCursorSelected.Invoke(pMove);
    }


}
