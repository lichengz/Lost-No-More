﻿using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Yarn.Unity;


/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>
public class StartGame : MonoBehaviour
{
	[SerializeField] private DialogueRunner _dialogueRunner = default;

	[SerializeField] private GameSceneSO _locationDebugGame;
	[SerializeField] private GameSceneSO _locationsToLoad;
	[SerializeField] private SaveSystem _saveSystem = default;
	[SerializeField] private bool _showLoadScreen = default;
	[SerializeField] private GameStateSO _gameState = default;
	[SerializeField] private QuestManagerSO _questManager = default;
	
	[Header("Broadcasting on")]
	[SerializeField] private LoadEventChannelSO _loadLocation = default;

	[Header("Listening to")]
	[SerializeField] private VoidEventChannelSO _onNewGameButton = default;
	[SerializeField] private VoidEventChannelSO _onContinueButton = default;
	[SerializeField] private VoidEventChannelSO _onDebugCardGame = default;
	[SerializeField] private VoidEventChannelSO _GameManagerStartEvent = default;

	private bool _hasSaveData;

	private void Start()
	{
		_hasSaveData = _saveSystem.LoadSaveDataFromDisk();
		_onNewGameButton.OnEventRaised += StartNewGame;
		_onContinueButton.OnEventRaised += ContinuePreviousGame;
		_dialogueRunner.onDialogueComplete.AddListener(StartNewGame);
		_onDebugCardGame.OnEventRaised += DebugCardGame;
	}

	private void OnDestroy()
	{
		_onNewGameButton.OnEventRaised -= StartNewGame;
		_onContinueButton.OnEventRaised -= ContinuePreviousGame;
		_onDebugCardGame.OnEventRaised -= DebugCardGame;
	}

	private void StartNewGame()
	{
		_hasSaveData = false;
		
		_saveSystem. WriteEmptySaveFile();
		_saveSystem.SetNewGameData();
		_loadLocation.RaiseEvent(_locationsToLoad, _showLoadScreen);
		_GameManagerStartEvent.OnEventRaised += OnStartOpenWorldGame;
	}

	private void ContinuePreviousGame()
	{
		StartCoroutine(LoadSaveGame());
	}

	private void DebugCardGame()
	{
		_gameState.UpdateGameState(GameState.CardGame);
		_loadLocation.RaiseEvent(_locationDebugGame, _showLoadScreen);
		_GameManagerStartEvent.OnEventRaised += OnStartCardGame;
	}

	private void OnResetSaveDataPress()
	{
		_hasSaveData = false;
	}

	private IEnumerator LoadSaveGame()
	{
		yield return StartCoroutine(_saveSystem.LoadSavedInventory());

		_saveSystem.LoadSavedQuestlineStatus(); 
		var locationGuid = _saveSystem.saveData._locationId;
		var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);

		yield return asyncOperationHandle;

		if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
		{
			LocationSO locationSO = asyncOperationHandle.Result;
			_loadLocation.RaiseEvent(locationSO, _showLoadScreen);
		}
	}
	
	void OnStartOpenWorldGame()
	{
		_gameState.UpdateGameState(GameState.Gameplay);
		_questManager.StartGame();
	}
	
	void OnStartCardGame()
	{
		_gameState.UpdateGameState(GameState.CardGame);
		_questManager.StartGame();
	}
}
