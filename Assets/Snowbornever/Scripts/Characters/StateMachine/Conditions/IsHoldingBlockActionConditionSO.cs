using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Holding Block Action")]
public class IsHoldingBlockActionConditionSO : StateConditionSO<IsHoldingBlockActionCondition> { }

public class IsHoldingBlockActionCondition : Condition
{
	//Component references
	private Protagonist _protagonistScript;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	protected override bool Statement()
	{
		return _protagonistScript.blockInput;
	}
}
