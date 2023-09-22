using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using System.Linq;

public class LookForTarget : CharacterAction
{
    [SerializeField]
    private string targetTag = "Player";
    
    public SharedTransform target;
    public SharedVector2 targetPos;
    public SharedVector2 dodgePos;
    public SharedFloat distanceToTarget;
    public SharedVector2 directionToTarget;

    private Transform targetTransform;

    public override TaskStatus OnUpdate()
    {
        targetTransform = FindClosestTarget();
        target.Value = targetTransform;
        if (targetTransform == null) return TaskStatus.Running;
        targetPos.Value = targetTransform.position;
        dodgePos.Value = transform.position + (transform.position - targetTransform.position).normalized * 5;
        distanceToTarget.Value = DistanceToTarget();
        directionToTarget.Value = DirectionToTarget();
        return TaskStatus.Success;
    }

    private Transform FindClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        if (targets.Length == 0)
        {
            return null;
        }

        return GameObject.FindGameObjectsWithTag(targetTag)
            .OrderBy(o => Vector3.Distance(o.transform.position, transform.position))
            .First().transform;
    }
    
    protected float DistanceToTarget()
    {
        if (targetTransform == null) return 0;
        return Vector3.Distance(transform.position, targetTransform.position);
    }
    
    private Vector2 DirectionToTarget()
    {
        if (targetTransform == null) return Vector2.zero;
        return (targetTransform.transform.position - transform.position).normalized;
    }
    
}