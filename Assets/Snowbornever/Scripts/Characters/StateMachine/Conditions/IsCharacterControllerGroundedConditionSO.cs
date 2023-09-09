using TopDownCharacter2D.Controllers;
using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Character Controller Grounded")]
public class IsCharacterControllerGroundedConditionSO : StateConditionSO<IsCharacterControllerGroundedCondition> { }

public class IsCharacterControllerGroundedCondition : Condition
{
	private TopDownCharacterController _topDownCharacterController;

	public override void Awake(StateMachine stateMachine)
	{
		_topDownCharacterController = stateMachine.GetComponent<TopDownCharacterController>();
	}

	protected override bool Statement() => _topDownCharacterController.isGrounded;
}
