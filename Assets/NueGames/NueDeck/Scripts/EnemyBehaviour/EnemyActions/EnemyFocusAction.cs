using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.EnemyBehaviour.EnemyActions
{
    public class EnemyFocusAction: EnemyActionBase
    {
        public override EnemyActionType ActionType => EnemyActionType.Focus;
        
        public override void DoAction(EnemyActionParameters actionParameters)
        {
            if (!actionParameters.TargetCharacter) return;
            int value = (int)actionParameters.Value;
            actionParameters.TargetCharacter.CharacterStats.Focus(value);
            if (FxManager != null)
            {
                FxManager.PlayFx(actionParameters.TargetCharacter.transform,FxType.Buff);
                FxManager.SpawnFloatingText(actionParameters.TargetCharacter.TextSpawnRoot,value.ToString());
            }

            if (NueAudioManager != null)
                NueAudioManager.PlayOneShot(AudioActionType.Power);
           
        }
    }
}