using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Manager/Move Trigger Manager")]
public class SO_EnemyMoveTriggerManager : ScriptableObject
{
    public UnityEvent<MoveStats> onMoveReachedEnd;

    public void MoveReachedEnd(MoveStats pMove)
    {
        onMoveReachedEnd.Invoke(pMove);
    }

}
