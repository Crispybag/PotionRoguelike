using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Manager/OnPlayerMoved")]
public class SO_OnPlayerMoved : ScriptableObject
{

    public UnityEvent<PlayerMovement> onPlayerMoved;

    public void OnPlayerMoved(PlayerMovement pPlayerMovement)
    {
        onPlayerMoved.Invoke(pPlayerMovement);
    }
}
