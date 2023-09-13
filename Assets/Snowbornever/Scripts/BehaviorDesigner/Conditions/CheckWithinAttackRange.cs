using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckWithinAttackRange : CharacterCondition
{
    [SerializeField] private float attackRange = 5f;
    
    public SharedFloat distanceToTarget;
    public override TaskStatus OnUpdate()
    {
        return distanceToTarget.Value <= attackRange ? TaskStatus.Success : TaskStatus.Failure;
    }
}
