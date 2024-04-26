using System;
using System.Collections;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.NueDeck.Scripts.Utils
{
    public class SceneChanger : MonoBehaviour
    {
        private NueGameManager NueGameManager => NueGameManager.Instance;
        private NueUIManager NueUIManager => NueUIManager.Instance;
        
        private enum SceneType
        {
            MainMenu,
            Map,
            Combat
        }
        public void OpenMainMenuScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.MainMenu));
        }
        private IEnumerator DelaySceneChange(SceneType type)
        {
            NueUIManager.SetCanvas(NueUIManager.Instance.InventoryCanvas,false,true);
            yield return StartCoroutine(NueUIManager.Instance.Fade(true));

            switch (type)
            {
                case SceneType.MainMenu:
                    NueUIManager.ChangeScene(NueGameManager.SceneData.mainMenuSceneIndex);
                    NueUIManager.SetCanvas(NueUIManager.CombatCanvas,false,true);
                    NueUIManager.SetCanvas(NueUIManager.InformationCanvas,false,true);
                    NueUIManager.SetCanvas(NueUIManager.RewardCanvas,false,true);
                   
                    NueGameManager.InitGameplayData();
                    NueGameManager.SetInitalHand();
                    break;
                case SceneType.Map:
                    NueUIManager.ChangeScene(NueGameManager.SceneData.mapSceneIndex);
                    NueUIManager.SetCanvas(NueUIManager.CombatCanvas,false,true);
                    NueUIManager.SetCanvas(NueUIManager.InformationCanvas,true,false);
                    NueUIManager.SetCanvas(NueUIManager.RewardCanvas,false,true);
                   
                    break;
                case SceneType.Combat:
                    NueUIManager.ChangeScene(NueGameManager.SceneData.combatSceneIndex);
                    NueUIManager.SetCanvas(NueUIManager.CombatCanvas,false,true);
                    NueUIManager.SetCanvas(NueUIManager.InformationCanvas,true,false);
                    NueUIManager.SetCanvas(NueUIManager.RewardCanvas,false,true);
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        public void OpenMapScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Map));
        }
        public void OpenCombatScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Combat));
        }
        public void ChangeScene(int sceneId)
        {

            if (sceneId == NueGameManager.SceneData.mainMenuSceneIndex)
                OpenMainMenuScene();
            else if (sceneId == NueGameManager.SceneData.mapSceneIndex)
                OpenMapScene();
            else if (sceneId == NueGameManager.SceneData.combatSceneIndex)
                OpenCombatScene();
            else
                SceneManager.LoadScene(sceneId);
            
            TooltipManager.Instance.HideTooltip();
        }
        public void ExitApp()
        {
            NueGameManager.OnExitApp();
            Application.Quit();
        }
    }
}
