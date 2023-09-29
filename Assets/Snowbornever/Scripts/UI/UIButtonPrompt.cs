using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIButtonPrompt : MonoBehaviour
{

	[SerializeField] private Image _interactionKeyBG = default;
	[SerializeField] private TextMeshProUGUI _interactionKeyText = default;
	[SerializeField] private Sprite _controllerSprite = default;
	[SerializeField] private Sprite _keyboardSprite = default;
	[SerializeField] private string _interactionKeyboardCode = default;
	[SerializeField] private string _interactionJoystickKeyCode = default;

	private void OnEnable()
	{
		SetButtonPrompt(Gamepad.current == null);
	}

	public void SetButtonPrompt(bool isKeyboard)
	{
		if (!isKeyboard)
		{
			_interactionKeyBG.sprite = _controllerSprite;
			_interactionKeyBG.type = Image.Type.Simple;
			_interactionKeyText.text = _interactionKeyText.text.Replace(' ', (char)160);
		}
		else
		{
			_interactionKeyBG.sprite = _keyboardSprite;
			_interactionKeyBG.type = Image.Type.Sliced;
			_interactionKeyText.text = _interactionKeyboardCode;
		}
	}
}
