using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem.Wrappers;

public class InteractUsable : Usable
{
    [SerializeField] public InteractionSO _customInteract= default;
    [SerializeField] private bool _ifTriggerInteractionEvent = true;
    [SerializeField] private BoolEventChannelSO _startInteractionEvent = default;
    [SerializeField] private bool _isReInteractable = default;
    public override void OnUseUsable()
    {
        base.OnUseUsable();
        if (_ifTriggerInteractionEvent)
        {
            _startInteractionEvent.RaiseEvent(_isReInteractable);
        }
    }
}
