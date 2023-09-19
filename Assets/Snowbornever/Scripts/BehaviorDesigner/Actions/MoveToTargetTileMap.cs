using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class MoveToTargetTileMap : CharacterAction
{
    public SharedVector2 targetPos;

    public override void OnStart()
    {
        characterController.OnMoveNavMeshEvent.Invoke(targetPos.Value);
    }

    public override TaskStatus OnUpdate()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return TaskStatus.Success;
                }
            }
        }
        return TaskStatus.Running;
    }
}
