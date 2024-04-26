using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInteraction : MonoBehaviour
{
    [SerializeField] private List<InteractionSO> _listInteractions = default;
    [SerializeField] Image _interactionIcon = default;
    [SerializeField] TextMeshProUGUI description;

    public void FillInteractionPanel(InteractionType interactionType, InteractionSO customInteract = null)
    {
        InteractionSO interact = customInteract;
        if (interact == null && _listInteractions != null
            && _listInteractions.Exists(o => o.InteractionType == interactionType))
        {
            interact = _listInteractions.Find(o => o.InteractionType == interactionType);
            
        }

        if (interact != null)
        {
            Sprite icon = interact.InteractionIcon;
            description.text = interact.InteractionName.GetLocalizedString();
        }
    }
}