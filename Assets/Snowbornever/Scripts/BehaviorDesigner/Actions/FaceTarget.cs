using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FaceTarget : CharacterAction
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
