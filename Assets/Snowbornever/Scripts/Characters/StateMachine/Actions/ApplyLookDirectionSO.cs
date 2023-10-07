using TopDownCharacter2D.Controllers;
using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ApplyLookDirection", menuName = "State Machines/Actions/Apply Look Direction")]
public class ApplyLookDirectionSO : StateActionSO<ApplyLookDirection> { }

public class ApplyLookDirection : StateAction
{
	//Component references
	private Protagonist _protagonistScript;
	private Animator _animator;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
		_animator = stateMachine.GetComponent<Animator>();
	}

	public override void OnUpdate()
	{
		if (_protagonistScript.lookVector.magnitude != 0)
		{
			_animator.SetFloat(Animator.StringToHash("moveX"), Mathf.RoundToInt(_protagonistScript.lookVector.x));
			_animator.SetFloat(Animator.StringToHash("moveY"), Mathf.RoundToInt(_protagonistScript.lookVector.y));
		}
	}
}
