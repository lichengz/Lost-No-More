using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckIsPlayerAttacking : CharacterCondition
{
    public override TaskStatus OnUpdate()
    {
        return SpawnSystem.Instance.playerInstance.IsAttacking ? TaskStatus.Success : TaskStatus.Failure;
    }
}
