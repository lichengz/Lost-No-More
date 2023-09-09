using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TopDownCharacter2D.Controllers;

public class MoveToTilemap : Action
{
	public SharedVector2 pos;
	private TopDownCharacterController _topDownCharacterController;

	public override void OnAwake()
	{
		_topDownCharacterController = GetComponent<TopDownCharacterController>();
	}
	public override void OnStart()
	{
		_topDownCharacterController.MovePath(pos.Value);
	}

	public override TaskStatus OnUpdate()
	{
		if (_topDownCharacterController.isReachEnd)
		{
			return TaskStatus.Success;
		}
		else
		{
			return TaskStatus.Running;
		}
	}
}