using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHelper : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _startInteractionEvent = default;
    [SerializeField] private IntEventChannelSO _closeUIDialogueEvent = default;

    public void OpenUIDialogueEvent()
    {
        _startInteractionEvent.RaiseEvent();
    }
    
    public void CloseUIDialogueEvent()
    {
        _closeUIDialogueEvent.RaiseEvent(0);
    }
}
