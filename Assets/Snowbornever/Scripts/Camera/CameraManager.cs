using System;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	public InputReader inputReader;
	public Camera mainCamera;
	public CinemachineVirtualCamera VCam;
	public CinemachineImpulseSource impulseSource;
	private bool _isRMBPressed;
	private Protagonist _protagonist;
	private CinemachineFramingTransposer _transposer;

	[SerializeField][Range(.5f, 3f)] private float _speedMultiplier = 1f; //TODO: make this modifiable in the game settings											
	[SerializeField] private TransformAnchor _cameraTransformAnchor = default;
	[SerializeField] private TransformAnchor _protagonistTransformAnchor = default;

	[Header("Listening on channels")]
	[Tooltip("The CameraManager listens to this event, fired by protagonist GettingHit state, to shake camera")]
	[SerializeField] private VoidEventChannelSO _camShakeEvent = default;

	private bool _cameraMovementLock = false;

	private void OnEnable()
	{
		_protagonistTransformAnchor.OnAnchorProvided += SetupProtagonistVirtualCamera;
		_camShakeEvent.OnEventRaised += impulseSource.GenerateImpulse;

		_cameraTransformAnchor.Provide(mainCamera.transform);
	}

	private void OnDisable()
	{
		_protagonistTransformAnchor.OnAnchorProvided -= SetupProtagonistVirtualCamera;
		_camShakeEvent.OnEventRaised -= impulseSource.GenerateImpulse;

		_cameraTransformAnchor.Unset();
	}

	private void Start()
	{
		//Setup the camera target if the protagonist is already available
		if(_protagonistTransformAnchor.isSet)
			SetupProtagonistVirtualCamera();
	}

	private void Update()
	{
		if (_protagonist != null)
		{
			if (_protagonist.inputVector.magnitude > 0)
			{
				_transposer.m_TrackedObjectOffset = _protagonist.inputVector * 0.3f;
			}
		}
	}

	/// <summary>
	/// Provides Cinemachine with its target, taken from the TransformAnchor SO containing a reference to the player's Transform component.
	/// This method is called every time the player is reinstantiated.
	/// </summary>
	public void SetupProtagonistVirtualCamera()
	{
		Transform target = _protagonistTransformAnchor.Value;
		_protagonist = _protagonistTransformAnchor.Value.GetComponent<Protagonist>();
		_transposer = VCam.GetCinemachineComponent<CinemachineFramingTransposer>();
		Vector3 tempFollow = VCam.Follow.position;
		VCam.Follow = target;
		VCam.OnTargetObjectWarped(target, target.position - tempFollow);
	}
}
