using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckMovingToWall : CharacterCondition
{
    public override TaskStatus OnUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, body.velocity, 1f, wallLayerMask);

        return hit.collider != null ? TaskStatus.Success : TaskStatus.Failure;
    }
}