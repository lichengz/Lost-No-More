using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NPCSelector : ProximitySelector
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
        if (usable == null)
        {
            _setInteractionEvent.RaiseEvent(false, type);
            return;
        }
        if (usable.GetComponent<DialogueSystemTrigger>() != null)
        {
            type = InteractionType.Talk;
        }
        _setInteractionEvent.RaiseEvent(true, type);
    }
}