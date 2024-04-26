using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.Localization;

public class InteractSelector : ProximitySelector
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;

    private void OnEnable()
    {
        _inputReader.InteractEvent += UseCurrentSelection;
    }

    //Removes all listeners to the events coming from the InputReader script
    private void OnDisable()
    {
        _inputReader.InteractEvent -= UseCurrentSelection;
    }

    protected override bool IsUseButtonDown()
    {
        // mute the original input, since I'm using the new Input System.
        return false;
    }

    public override void SetCurrentUsable(Usable usable)
    {
        base.SetCurrentUsable(usable);
        InteractionType type = InteractionType.None;

        InteractionSO _customInteract = null;
        if (usable is InteractUsable)
        {
            InteractUsable interactUsable = (InteractUsable)usable;
            _customInteract = interactUsable._customInteract;
        }
        
        if (_customInteract != null)
        {
            _setInteractionEvent.RaiseEvent(true, type, _customInteract);
        }
        else
        {
            if (usable == null)
            {
                _setInteractionEvent.RaiseEvent(false, type, _customInteract);
                return;
            }
            if (usable.GetComponent<DialogueSystemTrigger>() != null)
            {
                type = InteractionType.Talk;
            }else if (usable.GetComponent<BreakAndLoot>() != null)
            {
                type = InteractionType.PickUp;
            }
            _setInteractionEvent.RaiseEvent(true, type, _customInteract);
        }
    }
}