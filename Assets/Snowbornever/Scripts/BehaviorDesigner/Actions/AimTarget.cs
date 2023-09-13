using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AimTarget : CharacterAction
{
    public SharedVector2 directionToTarget;
    public override TaskStatus OnUpdate()
    {
        RotateArm(directionToTarget.Value);
        return TaskStatus.Success;
    }
    
    private void RotateArm(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        characterController.armRenderer.flipY = Mathf.Abs(rotZ) > 90f;
        foreach (SpriteRenderer charRenderer in characterController.characterRenderers)
        {
            charRenderer.flipX = characterController.armRenderer.flipY;
        }

        characterController.armPivot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
