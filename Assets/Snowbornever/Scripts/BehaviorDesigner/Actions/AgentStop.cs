using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AgentStop : CharacterAction
{
    public override TaskStatus OnUpdate()
    {
        agent.destination = transform.position;
        return TaskStatus.Success;
    }
}