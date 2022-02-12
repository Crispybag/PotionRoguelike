using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Managers/Move Trigger Manager")]
public class SO_MoveTriggerManager : ScriptableObject
{
    public UnityEvent<MoveStats> onMoveReachedEnd;

    public void MoveReachedEnd(MoveStats pMove)
    {
        onMoveReachedEnd.Invoke(pMove);
    }

}
