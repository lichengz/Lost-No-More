using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TopDownCharacter2D.Controllers;

public class MoveToTilemap : CharacterAction
{
	public override void OnStart()
	{
		if (characterController.Path.Count > 0)
		{
			Vector2 targetPos = characterController.tilemap.CellToWorld(characterController.Path[0]);
			float distance = DistanceFunc(targetPos, body.position);
			if (distance > 0.1f)
			{
				characterController.OnMoveEvent.Invoke((targetPos - body.position).normalized);
			}
			characterController.Path.RemoveAt(0);
		}
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
	
	private float DistanceFunc(Vector3 a, Vector3 b)
	{
		return (a - b).sqrMagnitude;
	}
}