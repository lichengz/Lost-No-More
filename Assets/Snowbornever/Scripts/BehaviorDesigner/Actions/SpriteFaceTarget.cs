using BehaviorDesigner.Runtime.Tasks;

public class SpriteFaceTarget : CharacterAction
{
    public override TaskStatus OnUpdate()
    {
        if (characterController.characterRenderers != null)
        {
            foreach (var sprite in characterController.characterRenderers)
            {
                sprite.flipX = agent.destination.x < transform.position.x;
            }
        }
        return TaskStatus.Success;
    }
}