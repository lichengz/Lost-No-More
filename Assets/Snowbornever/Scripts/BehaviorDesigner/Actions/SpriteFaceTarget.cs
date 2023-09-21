using BehaviorDesigner.Runtime.Tasks;

public class SpriteFaceTarget : CharacterAction
{
    public override TaskStatus OnUpdate()
    {
        if (characterController.characterRenderers != null)
        {
            foreach (var sprite in characterController.characterRenderers )
            {
                if (agent.destination.x < transform.position.x - 0.5f)
                {
                    sprite.flipX = true;
                }else if (agent.destination.x > transform.position.x + 0.5f)
                {
                    sprite.flipX = false;
                }
            }
        }
        return TaskStatus.Success;
    }
}