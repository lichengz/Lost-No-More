using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHelper : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private VoidEventChannelSO _startInteractionEvent = default;
    [SerializeField] private IntEventChannelSO _closeUIDialogueEvent = default;

    public void OpenUIDialogueEvent()
    {
        _inputReader.EnableDialogueInput();
        _startInteractionEvent.RaiseEvent();
    }
    
    public void CloseUIDialogueEvent()
    {
        _inputReader.EnableGameplayInput();
        _closeUIDialogueEvent.RaiseEvent(0);
    }
}
