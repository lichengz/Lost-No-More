using System;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NueGames.NueDeck.Scripts.Card
{
    public class ChoiceCard : MonoBehaviour,IPointerEnterHandler,IPointerDownHandler,IPointerExitHandler,IPointerUpHandler
    {
        [SerializeField] private float showScaleRate = 1.15f;
        private CardBase _cardBase;
        private Vector3 _initalScale;
        public Action OnCardChose;
        public NueGameManager NueGameManager => NueGameManager.Instance;
        public NueUIManager NueUIManager => NueUIManager.Instance;
        
        public void BuildReward(CardData cardData)
        {
            _cardBase = GetComponent<CardBase>();
            _initalScale = transform.localScale;
            _cardBase.SetCard(cardData);
            _cardBase.UpdateCardText();
        }


        private void OnChoice()
        {
            if (NueGameManager != null)
                NueGameManager.PersistentGameplayData.CurrentCardsList.Add(_cardBase.CardData);

            if (NueUIManager != null)
                NueUIManager.RewardCanvas.ChoicePanel.DisablePanel();
            OnCardChose?.Invoke();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = _initalScale * showScaleRate;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
           
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _initalScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnChoice();
            
        }
    }
}
