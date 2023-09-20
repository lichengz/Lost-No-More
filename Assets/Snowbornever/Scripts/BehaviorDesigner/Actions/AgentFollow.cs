using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AgentFollow : CharacterAction
{
    public SharedTransform agentDestination;

    public override void OnStart()
    {
        if (agentDestination.Value != null)
        {
            agent.destination = agentDestination.Value.position;
            animator.SetBool(IsWalking, true);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (characterController.characterRenderers != null)
        {
            foreach (var sprite in characterController.characterRenderers )
            {
                sprite.flipX = agent.destination.x < transform.position.x;
            }
        }

        if (agent.remainingDistance <= float.Epsilon || agent.velocity.magnitude <= float.Epsilon)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        animator.SetBool(IsWalking, false);
    }
}