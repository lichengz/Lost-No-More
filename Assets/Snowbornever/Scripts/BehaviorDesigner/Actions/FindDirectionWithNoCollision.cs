using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class FindDirectionWithNoCollision : CharacterAction
{
    [SerializeField]
    private LayerMask layerMask = LayerMask.GetMask("interactable");
    public SharedVector2 direction;
    List<Vector2> openDirs = new List<Vector2>();

    public override TaskStatus OnUpdate()
    {
        Vector2[] dirs = { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
        foreach (Vector2 dir in dirs)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1, layerMask);
            if (hit.collider == null)
            {
                openDirs.Add(dir);

            }
        }

        if (openDirs.Count > 0)
        {
            direction.Value = openDirs[Random.Range(0, openDirs.Count)];
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        openDirs.Clear();
    }
}