using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIHealthBarManager : MonoBehaviour
{
	[SerializeField] private HealthSO _protagonistHealth = default; //the HealthBar is watching this object, which is the health of the player
	[SerializeField] private HealthConfigSO _healthConfig = default;
	
	[SerializeField] private Slider healthSlider;
	[SerializeField] private UnityEvent onHealthUpdate;
	
	[Header("Listening to")]
	[SerializeField] private VoidEventChannelSO _UIUpdateNeeded = default; //The player's Damageable issues this

	private void OnEnable()
	{
		_UIUpdateNeeded.OnEventRaised += UpdateHeartImages;
		
		InitializeHealthBar();
	}

	private void OnDestroy()
	{
		_UIUpdateNeeded.OnEventRaised -= UpdateHeartImages;
	}

	private void InitializeHealthBar()
	{
		_protagonistHealth.SetMaxHealth(_healthConfig.InitialHealth);
		_protagonistHealth.SetCurrentHealth(_healthConfig.InitialHealth);

		UpdateHeartImages();
	}

	private void UpdateHeartImages()
	{
		healthSlider.value = (float)_protagonistHealth.CurrentHealth / _protagonistHealth.MaxHealth;
		onHealthUpdate.Invoke();
	}
}
