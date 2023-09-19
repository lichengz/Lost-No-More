using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Character;
using DG.Tweening;
using UnityEngine;

public class ApplyMovement : CharacterAction
{
    public SharedVector2 direction;
    public float duration;

    private Tween movementTween;

    public override void OnStart()
    {
        // characterController.OnMoveEvent.Invoke(direction.Value);
        // movementTween = DOVirtual.DelayedCall(duration, () =>
        // {
        //     characterController.OnMoveEvent.Invoke(Vector2.zero);
        // }, false);
    }

    public override TaskStatus OnUpdate()
    {
        return movementTween.IsActive() && movementTween.IsPlaying() ? TaskStatus.Running : TaskStatus.Success;
    }

    public override void OnEnd()
    {
        movementTween?.Kill();
    }
}