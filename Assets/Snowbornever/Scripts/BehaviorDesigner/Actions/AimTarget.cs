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

        if (characterController.GetWeaponType() == WeaponController.WeaponType.Melee)
        {
            characterController.weaponRenderer.flipX = Mathf.Abs(rotZ) > 90f;
        }
        else
        {
            characterController.weaponRenderer.flipY = Mathf.Abs(rotZ) > 90f;
            foreach (SpriteRenderer charRenderer in characterController.characterRenderers)
            {
                charRenderer.flipX = characterController.weaponRenderer.flipY;
            }

            characterController.weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }
    }
}
