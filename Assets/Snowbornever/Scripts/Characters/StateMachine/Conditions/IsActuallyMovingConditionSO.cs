using TopDownCharacter2D.Controllers;
using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsActuallyMoving", menuName = "State Machines/Conditions/Is Actually Moving")]
public class IsActuallyMovingConditionSO : StateConditionSO
{
	[SerializeField] private float _treshold = 0.02f;

	protected override Condition CreateCondition() => new IsActuallyMovingCondition(_treshold);
}

public class IsActuallyMovingCondition : Condition
{
	private float _treshold;
	private TopDownCharacterController _topDownCharacterController;

	public override void Awake(StateMachine stateMachine)
	{
		_topDownCharacterController = stateMachine.GetComponent<TopDownCharacterController>();
	}

	public IsActuallyMovingCondition(float treshold)
	{
		_treshold = treshold;
	}

	protected override bool Statement()
	{
		return _topDownCharacterController.velocity.sqrMagnitude > _treshold * _treshold;
	}
}
