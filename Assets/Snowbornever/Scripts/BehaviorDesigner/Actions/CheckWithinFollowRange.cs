using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckWithinFollowRange : CharacterAction
{
    [SerializeField] private float followRange = 10f;
    
    public SharedFloat distanceToTarget;
    public override TaskStatus OnUpdate()
    {
        return distanceToTarget.Value <= followRange ? TaskStatus.Success : TaskStatus.Failure;
    }
}
