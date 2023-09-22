using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ResetGetHitState", menuName = "State Machines/Actions/Reset Get Hit State")]
public class ResetGetHitStateSO : StateActionSO
{
	protected override StateAction CreateAction() => new ResetGetHitState();
}

public class ResetGetHitState : StateAction
{
	private Damageable _damageableEntity;
	private Protagonist _protagonist;

	public override void Awake(StateMachine stateMachine)
	{
		_damageableEntity = stateMachine.GetComponent<Damageable>();
		_protagonist = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnStateEnter()
	{
		_protagonist.IsAttacking = false;
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateExit()
	{
		_damageableEntity.GetHit = false;
	}
}
