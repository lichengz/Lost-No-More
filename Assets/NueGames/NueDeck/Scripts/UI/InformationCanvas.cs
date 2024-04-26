using NueGames.NueDeck.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.UI
{
    public class InformationCanvas : CanvasBase
    {
        [Header("Settings")] 
        [SerializeField] private GameObject randomizedDeckObject;
        [SerializeField] private TextMeshProUGUI roomTextField;
        [SerializeField] private TextMeshProUGUI goldTextField;
        [SerializeField] private TextMeshProUGUI nameTextField;
        [SerializeField] private TextMeshProUGUI healthTextField;

        public GameObject RandomizedDeckObject => randomizedDeckObject;
        public TextMeshProUGUI RoomTextField => roomTextField;
        public TextMeshProUGUI GoldTextField => goldTextField;
        public TextMeshProUGUI NameTextField => nameTextField;
        public TextMeshProUGUI HealthTextField => healthTextField;
        
        
        #region Setup
        private void Awake()
        {
            ResetCanvas();
        }
        #endregion
        
        #region Public Methods
        public void SetRoomText(int roomNumber,bool useStage = false, int stageNumber = -1) => 
            RoomTextField.text = useStage ? $"Room {stageNumber}/{roomNumber}" : $"Room {roomNumber}";

        public void SetGoldText(int value)=>GoldTextField.text = $"{value}";

        public void SetNameText(string name) => NameTextField.text = $"{name}";

        public void SetHealthText(int currentHealth,int maxHealth) => HealthTextField.text = $"{currentHealth}/{maxHealth}";

        public override void ResetCanvas()
        {
            RandomizedDeckObject.SetActive(NueGameManager.PersistentGameplayData.IsRandomHand);
            SetHealthText(NueGameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth,NueGameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth);
            SetNameText(NueGameManager.GameplayData.DefaultName);
            SetRoomText(NueGameManager.PersistentGameplayData.CurrentEncounterId+1,NueGameManager.GameplayData.UseStageSystem,NueGameManager.PersistentGameplayData.CurrentStageId+1);
            NueUIManager.InformationCanvas.SetGoldText(NueGameManager.PersistentGameplayData.CurrentGold);
        }
        #endregion
        
    }
}