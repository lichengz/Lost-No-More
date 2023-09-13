using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

public class RandomColliderPosWithInRange: Action
{
	public SharedVector2 pos;
	public float range = 5f;
	
	public  LayerMask mask = 0;

	public override TaskStatus OnUpdate()
	{
		Collider2D[] colliders = new Collider2D[5];
		Physics2D.OverlapCircleNonAlloc(transform.position, range, colliders, mask);
		
		List<Collider2D> realColliders = new List<Collider2D>();
		foreach (var c in colliders)
		{
			if (c != null) realColliders.Add(c);
		}
		colliders = realColliders.ToArray();
		int index = Random.Range(0, realColliders.Count);
		pos.Value = realColliders[index].transform.position;
		return TaskStatus.Success;
	}
}