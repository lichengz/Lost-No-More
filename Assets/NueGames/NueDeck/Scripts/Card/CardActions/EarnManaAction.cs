using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class EarnManaAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.EarnMana;
        public override void DoAction(CardActionParameters actionParameters)
        {
            if (CombatManager != null)
                CombatManager.IncreaseMana(Mathf.RoundToInt(actionParameters.Value));
            else
                Debug.LogError("There is no CombatManager");

            actionParameters.SelfCharacter.CharacterStats.EarnFocus(Mathf.RoundToInt(actionParameters.Value));
            if (FxManager != null)
                FxManager.PlayFx(actionParameters.SelfCharacter.transform, FxType.Buff);
            
            if (NueAudioManager != null) 
                NueAudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}