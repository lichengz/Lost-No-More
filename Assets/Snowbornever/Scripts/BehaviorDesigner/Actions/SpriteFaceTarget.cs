using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class SpriteFaceTarget : CharacterAction
{
    public override TaskStatus OnUpdate()
    {
        if (agent.remainingDistance <= float.Epsilon) return TaskStatus.Success;
        if (characterController.characterRenderers != null)
        {
            foreach (var sprite in characterController.characterRenderers)
            {
                // sprite.flipX = agent.destination.x < transform.position.x;
                bool flip = agent.destination.x < transform.position.x;
                if (flip && !sprite.flipX)
                {
                    sprite.flipX = true;
                }else if (!flip && sprite.flipX)
                {
                    sprite.flipX = false;
                }
            }
        }
        return TaskStatus.Success;
    }
}