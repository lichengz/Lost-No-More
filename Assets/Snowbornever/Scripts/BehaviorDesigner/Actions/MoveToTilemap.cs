using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TopDownCharacter2D.Controllers;

public class MoveToTilemap : CharacterAction
{
	public SharedVector2 pos;

	public override void OnStart()
	{
		characterController.MovePath(pos.Value);
	}

	public override TaskStatus OnUpdate()
	{
		if (characterController.isReachEnd)
		{
			return TaskStatus.Success;
		}
		else if(body.velocity.magnitude < 0.1f)
		{
			// run into a wall
			return TaskStatus.Failure;
		}
		else
		{
			return TaskStatus.Running;
		}
	}
}