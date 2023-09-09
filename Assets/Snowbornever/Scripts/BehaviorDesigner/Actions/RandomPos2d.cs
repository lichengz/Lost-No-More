using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class RandomPos2d : Action
{
	public SharedVector2 pos;
	public Vector2 min = Vector2.one * -5;
	public Vector2 max = Vector2.one * 5;

	public override TaskStatus OnUpdate()
	{
		pos.Value = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
		return TaskStatus.Success;
	}
}