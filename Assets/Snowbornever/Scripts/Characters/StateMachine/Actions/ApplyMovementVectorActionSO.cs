using TopDownCharacter2D.Controllers;
using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ApplyMovementVector", menuName = "State Machines/Actions/Apply Movement Vector")]
public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVectorAction>
{
}

public class ApplyMovementVectorAction : StateAction
{
    //Component references
    private Protagonist _protagonistScript;
    private TopDownCharacterController _topDownCharacterController;
    private Animator _animator;

    public override void Awake(StateMachine stateMachine)
    {
        _protagonistScript = stateMachine.GetComponent<Protagonist>();
        _topDownCharacterController = stateMachine.GetComponent<TopDownCharacterController>();
        _animator = stateMachine.GetComponent<Animator>();
    }

    public override void OnUpdate()
    {
        _topDownCharacterController.OnMoveEvent.Invoke(_protagonistScript.movementVector);
        _protagonistScript.movementVector = _topDownCharacterController.velocity;
        if (_protagonistScript.movementInput.magnitude != 0)
        {
            _animator.SetFloat(Animator.StringToHash("moveX"), _protagonistScript.movementInput.x);
            _animator.SetFloat(Animator.StringToHash("moveY"), _protagonistScript.movementInput.y);
        }
    }
}