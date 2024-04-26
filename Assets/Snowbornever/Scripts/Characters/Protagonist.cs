using System;
using System.Collections;
using TopDownCharacter2D.Attacks;
using TopDownCharacter2D.Controllers;
using UnityEngine;

/// <summary>
/// <para>This component consumes input on the InputReader and stores its values. The input is then read, and manipulated, by the StateMachines's Actions.</para>
/// </summary>
public class Protagonist : TopDownCharacterController
{
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private TransformAnchor _gameplayCameraTransform = default;
	[SerializeField] private Transform _syncPlayerTransform = default;

	private Vector2 _inputVector;
	private float _previousSpeed;

	//These fields are read and manipulated by the StateMachine actions
	[NonSerialized] public bool jumpInput;
	[NonSerialized] public bool extraActionInput;
	
	public Vector2 inputVector => _inputVector;
	public bool attackInput => IsAttacking;
	public bool blockInput => IsBlocking;
	[NonSerialized] public Vector2 movementInput; //Initial input coming from the Protagonist script
	[NonSerialized] public Vector2 movementVector; //Final movement vector, manipulated by the StateMachine actions
	[NonSerialized] public Vector2 lookVector; //Final movement vector, manipulated by the StateMachine actions
	[NonSerialized] public ControllerColliderHit lastHit;
	[NonSerialized] public bool isRunning; // Used when using the keyboard to run, brings the normalised speed to 1

	public const float GRAVITY_MULTIPLIER = 5f;
	public const float MAX_FALL_SPEED = -50f;
	public const float MAX_RISE_SPEED = 100f;
	public const float GRAVITY_COMEBACK_MULTIPLIER = .03f;
	public const float GRAVITY_DIVIDER = .6f;
	public const float AIR_RESISTANCE = 5f;
	
	[SerializeField] private EquipMeleeWeaponEventChannelSO equipMeleeWeapon;
	[SerializeField] private EquipRangeWeaponEventChannelSO equipRangeWeapon;
	[SerializeField] private VoidEventChannelSO equipDefaultWeapon;

	private void InitializePlayerWeapon(AttackConfig attackConfig)
	{
		foreach (Transform child in weaponPivot) {
			Destroy(child.gameObject);
		}
		Stats.CurrentStats.attackConfig = attackConfig;
		GameObject weapon = Instantiate(attackConfig.weaponPrefab, weaponPivot);
		WeaponController weaponController = weapon.GetComponent<WeaponController>();
		weaponRenderer = weaponController.weaponRenderer;
		projectileSpawnPosition = weaponController.projectileSpawnPosition;
		isWeaponInited = true;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		lastHit = hit;
	}

	//Adds listeners for events being triggered in the InputReader script
	private void OnEnable()
	{
		_inputReader.JumpEvent += OnJumpInitiated;
		_inputReader.JumpCanceledEvent += OnJumpCanceled;
		_inputReader.MoveEvent += OnMove;
		_inputReader.LookMouseEvent += OnLookMouse;
		_inputReader.LookStickEvent += OnLookStick;
		_inputReader.StartedRunning += OnStartedRunning;
		_inputReader.StoppedRunning += OnStoppedRunning;
		_inputReader.AttackEvent += OnStartedAttack;
		_inputReader.BlockEvent += OnBlockdAttack;
		equipMeleeWeapon.OnEventRaised += InitializePlayerWeapon;
		equipRangeWeapon.OnEventRaised += InitializePlayerWeapon;
		//...
	}

	//Removes all listeners to the events coming from the InputReader script
	private void OnDisable()
	{
		_inputReader.JumpEvent -= OnJumpInitiated;
		_inputReader.JumpCanceledEvent -= OnJumpCanceled;
		_inputReader.MoveEvent -= OnMove;
		_inputReader.LookMouseEvent -= OnLookMouse;
		_inputReader.LookStickEvent -= OnLookStick;
		_inputReader.StartedRunning -= OnStartedRunning;
		_inputReader.StoppedRunning -= OnStoppedRunning;
		_inputReader.AttackEvent -= OnStartedAttack;
		_inputReader.BlockEvent -= OnBlockdAttack;
		equipMeleeWeapon.OnEventRaised -= InitializePlayerWeapon;
		equipRangeWeapon.OnEventRaised -= InitializePlayerWeapon;
		//...
	}

	private IEnumerator Start()
	{
		while (equipDefaultWeapon.OnEventRaised == null)
		{
			yield return null;
		}
		equipDefaultWeapon.RaiseEvent();
	}

	private void Update()
	{
		base.Update();
		RecalculateMovement();
	}
	
	
	protected void UpdateFaceDirection()
	{
		_animator.SetFloat(Animator.StringToHash("moveX"), lookVector.x);
		_animator.SetFloat(Animator.StringToHash("moveY"), lookVector.y);
	}

	private void RecalculateMovement()
	{
		float targetSpeed;
		Vector2 adjustedMovement = new Vector2(_inputVector.x, _inputVector.y);

		// if (_gameplayCameraTransform.isSet)
		// {
		// 	//Get the two axes from the camera and flatten them on the XZ plane
		// 	Vector3 cameraForward = _gameplayCameraTransform.Value.forward;
		// 	cameraForward.y = 0f;
		// 	Vector3 cameraRight = _gameplayCameraTransform.Value.right;
		// 	cameraRight.y = 0f;
		//
		// 	//Use the two axes, modulated by the corresponding inputs, and construct the final vector
		// 	adjustedMovement = cameraRight.normalized * _inputVector.x +
		// 	                   cameraForward.normalized * _inputVector.y;
		// }
		// else
		// {
		// 	//No CameraManager exists in the scene, so the input is just used absolute in world-space
		// 	Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
		// 	adjustedMovement = new Vector3(_inputVector.x, 0f, _inputVector.y);
		// }

		//Fix to avoid getting a Vector3.zero vector, which would result in the player turning to x:0, z:0
		if (_inputVector.sqrMagnitude == 0f)
			adjustedMovement = transform.forward * (adjustedMovement.magnitude + .01f);

		//Accelerate/decelerate
		targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
		if (targetSpeed > 0f)
		{
			// This is used to set the speed to the maximum if holding the Shift key,
			// to allow keyboard players to "run"
			if (isRunning)
				targetSpeed = 1f;

			if (blockInput)
				targetSpeed = 0.1f;
			_syncPlayerTransform.localPosition = _inputVector * 0.3f;
		}
		
		targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, targetSpeed < _previousSpeed ? Time.deltaTime * 4f : Time.deltaTime * 10f);

		movementInput = adjustedMovement.normalized * targetSpeed;

		_previousSpeed = targetSpeed;

	}

	//---- EVENT LISTENERS ----

	private void OnMove(Vector2 movement)
	{

		_inputVector = movement;
	}

	private void OnJumpInitiated()
	{
		// jumpInput = true;
	}

	private void OnJumpCanceled()
	{
		jumpInput = false;
	}

	private void OnStoppedRunning() => isRunning = false;

	private void OnStartedRunning() => isRunning = true;


	private void OnStartedAttack()
	{
		UpdateFaceDirection();
		HandleAttackDelay();
	}
	
	private void OnBlockdAttack()
	{
		UpdateFaceDirection();
		IsBlocking = true;
	}

	// Triggered from Animation Event
	public void ExecuteAttack()
	{
		OnAttackEvent.Invoke(Stats.CurrentStats.attackConfig);
	}
	public void ConsumeAttackInput()
	{
		IsAttacking = false;
	}

	public void ConsumeBlockInput()
	{
		IsBlocking = false;
	}

	private void OnLookMouse(Vector2 look)
	{
		lookVector = HandleAimWithMouse(look);
	}
	private void OnLookStick(Vector2 look)
	{
		lookVector = HandleAimWithController(look);
	}
}
