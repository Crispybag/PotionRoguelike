using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientMovement : Movement
{
    [SerializeField] GridIngredient gridIngredient;
    bool isFalling;
    //[SerializeField]float distToDestroy = 0.3f;
    protected override void updateLerp(Vector3 walkDir)
    {
        base.updateLerp(walkDir);
        isFalling = gridIngredient.checkForFallAndRemove(walkDir);
    }

    protected override void Update()
    {
        base.Update();
        if (_lerpVal > _fastTapLimiter && isFalling) gridIngredient.DestroyIngredient(_endPosition - startPosition);
    }

}
