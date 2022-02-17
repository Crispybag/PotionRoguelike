using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Manager/PlayerMoveTriggerManager")]
public class SO_PlayerMoveManager : ScriptableObject
{
    public UnityEvent<PotionStats> onMoveReachedEnd;

    public void MoveReachedEnd(PotionStats pMove)
    {
        onMoveReachedEnd.Invoke(pMove);
    }

}