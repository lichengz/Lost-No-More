using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class RushTowardTarget : CharacterAction
{
    public SharedTransform target;
    public float force = 100;
    public float buildupTime = 0.5f;
    public float rushTime = 1f;
    private bool stopped = false;

    private Tween buildupTween;
    private Tween rushTween;

    public override void OnStart()
    {
        agent.enabled = false;
        buildupTween = DOVirtual.DelayedCall(buildupTime, StartRush);
    }

    private void StartRush()
    {
        body.AddForce((target.Value.position - transform.position).normalized * force, ForceMode2D.Impulse);

        rushTween = DOVirtual.DelayedCall(rushTime, () =>
        {
            stopped = true;
        });
    }

    public override TaskStatus OnUpdate()
    {
        return stopped ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        agent.enabled = true;
        buildupTween?.Kill();
        rushTween?.Kill();
        stopped = false;
    }
}
