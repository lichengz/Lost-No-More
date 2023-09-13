using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TopDownCharacter2D.Controllers;

public class FindPathOnTilemap : CharacterAction
{
	public SharedVector2 pos;

	public SharedInt stepsInPath;

	public override void OnStart()
	{
		characterController.SchedulePath(pos.Value);
		stepsInPath.Value = characterController.Path.Count;
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}