using System;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Utils
{
    public class InventoryHelper : MonoBehaviour
    {
        [SerializeField] private InventoryTypes inventoryType;
        
        private NueUIManager NueUIManager => NueUIManager.Instance;
        
        public void OpenInventory()
        {
            switch (inventoryType)
            {
                case InventoryTypes.CurrentDeck:
                    NueUIManager.OpenInventory(NueGameManager.Instance.PersistentGameplayData.CurrentCardsList,"Current Cards");
                    break;
                case InventoryTypes.DrawPile:
                    NueUIManager.OpenInventory(CollectionManager.Instance.DrawPile,"Draw Pile");
                    break;
                case InventoryTypes.DiscardPile:
                    NueUIManager.OpenInventory(CollectionManager.Instance.DiscardPile,"Discard Pile");
                    break;
                case InventoryTypes.ExhaustPile:
                    NueUIManager.OpenInventory(CollectionManager.Instance.ExhaustPile,"Exhaust Pile");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }
}