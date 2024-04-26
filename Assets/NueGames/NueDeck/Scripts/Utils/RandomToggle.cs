using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Utils
{
    public class RandomToggle : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;

        private NueGameManager NueGameManager => NueGameManager.Instance;
        public void CheckToggle()
        {
            NueGameManager.PersistentGameplayData.IsRandomHand = toggle.isOn;
            NueGameManager.SetInitalHand();
        }
    }
}