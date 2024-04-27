using System;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.NueDeck.Scripts.Utils
{
    [DefaultExecutionOrder(-11)]
    public class CoreLoader : MonoBehaviour
    {
        [SerializeField] private GameSceneSO _cardCore;
        private void Awake()
        {
            try
            {
                if (!NueGameManager.Instance)
                    _cardCore.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                Destroy(gameObject);
            }
            catch (Exception e)
            {
                Debug.LogError("You should add NueCore scene to build settings!");
                throw;
            }
           
        }
    }
}