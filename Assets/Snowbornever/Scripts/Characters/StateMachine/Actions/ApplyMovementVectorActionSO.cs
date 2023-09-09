using TopDownCharacter2D.Controllers;
using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ApplyMovementVector", menuName = "State Machines/Actions/Apply Movement Vector")]
public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVectorAction> { }

public class ApplyMovementVectorAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;
	private TopDownCharacterController _topDownCharacterController;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
		_topDownCharacterController = stateMachine.GetComponent<TopDownCharacterController>();
	}

	public override void OnUpdate()
	{
		_topDownCharacterController.OnMoveEvent.Invoke(_protagonistScript.movementInput);
		_protagonistScript.movementVector = _topDownCharacterController.velocity;
	}
}