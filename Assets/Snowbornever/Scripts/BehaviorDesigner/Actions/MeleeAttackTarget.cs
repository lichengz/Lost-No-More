using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TopDownCharacter2D.Attacks;
using TopDownCharacter2D.Attacks.Melee;

public class MeleeAttackTarget : CharacterAction
{
    public SharedVector2 directionToTarget;
    public MeleeAttackConfig attackConfig;
    [SerializeField] private GameObject attackObject;
    [SerializeField] private string attackAnimationTrigger;

    public override TaskStatus OnUpdate()
    {
        characterController.OnAttackEvent.Invoke(attackConfig);
        InstantiateAttack(attackConfig);
        animator.SetTrigger(attackAnimationTrigger);
        return TaskStatus.Success;
    }
    
    private void InstantiateAttack(MeleeAttackConfig attackConfig)
    {
        Transform attackPivot = characterController.projectileSpawnPosition;
        attackPivot.localRotation = Quaternion.identity;
        GameObject obj = GameObject.Instantiate(attackObject, attackPivot.position,
            Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, directionToTarget.Value)), attackPivot);
        MeleeAttackController attackController = obj.GetComponent<MeleeAttackController>();
        attackController.InitializeAttack(attackConfig, characterController.weaponRenderer);
    }
}
