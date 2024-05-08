using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Characters.Enemies;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Utils.Background;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using DialogueSystemTrigger = PixelCrushers.DialogueSystem.Wrappers.DialogueSystemTrigger;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class CombatManager : MonoBehaviour
    {
        private CombatManager(){}
        public static CombatManager Instance { get; private set; }

        [Header("References")] 
        [SerializeField] private BackgroundContainer backgroundContainer;
        [SerializeField] private List<Transform> enemyPosList;
        [SerializeField] private List<Transform> allyPosList;
        [SerializeField] protected VoidEventChannelSO startCombatEvent;
        [SerializeField] private IntEventChannelSO _closeUIDialogueEvent = default;
 
        
        #region Cache
        public List<EnemyBase> CurrentEnemiesList { get; private set; } = new List<EnemyBase>();
        public List<AllyBase> CurrentAlliesList { get; private set; }= new List<AllyBase>();

        public Action OnAllyTurnStarted;
        public Action OnEnemyTurnStarted;
        public List<Transform> EnemyPosList => enemyPosList;

        public List<Transform> AllyPosList => allyPosList;

        public AllyBase CurrentMainAlly => CurrentAlliesList.Count>0 ? CurrentAlliesList[0] : null;

        public EnemyEncounter CurrentEncounter { get; private set; }
        
        public CombatStateType CurrentCombatStateType
        {
            get => _currentCombatStateType;
            private set
            {
                ExecuteCombatState(value);
                _currentCombatStateType = value;
            }
        }
        
        private CombatStateType _currentCombatStateType;
        protected FxManager FxManager => FxManager.Instance;
        protected NueAudioManager NueAudioManager => NueAudioManager.Instance;
        protected NueGameManager NueGameManager => NueGameManager.Instance;
        protected NueUIManager NueUIManager => NueUIManager.Instance;

        protected CollectionManager CollectionManager => CollectionManager.Instance;
        
        private Queue<KeyValuePair<CardActionType, IEnumerator>> playerActionQueue = new Queue<KeyValuePair<CardActionType, IEnumerator>> ();
        public Queue<KeyValuePair<CardActionType, IEnumerator>> PlayerActionQueue => playerActionQueue;
        private Queue<IEnumerator> playerFocusActionQueue = new Queue<IEnumerator> ();
        public Queue<IEnumerator> PlayerFocusActionQueue => playerFocusActionQueue;
        private Queue<IEnumerator> playerPrepareActionQueue = new Queue<IEnumerator> ();
        public Queue<IEnumerator> PlayerPrepareActionQueue => playerPrepareActionQueue;
        private Queue<IEnumerator> playerAttackActionQueue = new Queue<IEnumerator> ();
        public Queue<IEnumerator> PlayerAttackActionQueue => playerAttackActionQueue;
        
        private Queue<KeyValuePair<EnemyIntentionType, IEnumerator>> enemyActionQueue = new Queue<KeyValuePair<EnemyIntentionType, IEnumerator>> ();
        public Queue<KeyValuePair<EnemyIntentionType, IEnumerator>> EnemyActionQueue => enemyActionQueue;
        private Queue<IEnumerator> enemyFocusActionQueue = new Queue<IEnumerator> ();
        public Queue<IEnumerator> EnemyFocusActionQueue => enemyFocusActionQueue;
        private Queue<IEnumerator> enemyPrepareActionQueue = new Queue<IEnumerator> ();
        public Queue<IEnumerator> EnemyPrepareActionQueue => enemyPrepareActionQueue;
        private Queue<IEnumerator> enemyAttackActionQueue = new Queue<IEnumerator> ();
        public Queue<IEnumerator> EnemyAttackActionQueue => enemyAttackActionQueue;

        #endregion
        
        
        #region Setup
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            } 
            else
            {
                Instance = this;
                CurrentCombatStateType = CombatStateType.PrepareCombat;
            }
        }

        private void Start()
        {
            StartCombat();
            PrepareDialogue();
        }

        public void StartCombat()
        {
            BuildEnemies();
            BuildAllies();
            backgroundContainer.OpenSelectedBackground();
          
            CollectionManager.SetGameDeck();
           
            NueUIManager.CombatCanvas.gameObject.SetActive(true);
            NueUIManager.InformationCanvas.gameObject.SetActive(true);
            // CurrentCombatStateType = CombatStateType.AllyTurn;
            startCombatEvent.OnEventRaisedOnce(() =>
            {
                CurrentCombatStateType = CombatStateType.AllyTurn;
            });
        }

        private void PrepareDialogue()
        {
            // reset Dialogue variables
            DialogueLua.SetVariable("EnemyIsHurt", false);
            
            // Dialogue trigger
            bool isDialogueTrigger = false;
            foreach (var currentEnemy in CurrentEnemiesList)
            {
                DialogueSystemTrigger dialogueTrigger = currentEnemy.GetComponent<DialogueSystemTrigger>();
                if (dialogueTrigger&& dialogueTrigger.isActiveAndEnabled)
                {
                    dialogueTrigger.OnUse();
                    isDialogueTrigger = true;
                }
            }

            if (!isDialogueTrigger)
            {
                CurrentCombatStateType = CombatStateType.AllyTurn;
            }
        }
        
        private void ExecuteCombatState(CombatStateType targetStateType)
        {
            switch (targetStateType)
            {
                case CombatStateType.PrepareCombat:
                    break;
                case CombatStateType.AllyTurn:

                    OnAllyTurnStarted?.Invoke();
                    
                    if (CurrentMainAlly.CharacterStats.IsStunned)
                    {
                        EndTurn();
                        return;
                    }

#if AUTO_MANA
                    NueGameManager.PersistentGameplayData.CurrentMana = NueGameManager.PersistentGameplayData.MaxMana;
#endif
                   
                    CollectionManager.DrawCards(NueGameManager.PersistentGameplayData.DrawCount);
                    
                    NueGameManager.PersistentGameplayData.CanSelectCards = true;
                    
                    break;
                case CombatStateType.EnemyTurn:

                    OnEnemyTurnStarted?.Invoke();
                    
                    CollectionManager.DiscardHand();
                    
                    StartCoroutine(nameof(EnemyTurnRoutine));
                    
                    NueGameManager.PersistentGameplayData.CanSelectCards = false;
                    
                    break;
                case CombatStateType.EndCombat:
                    
                    NueGameManager.PersistentGameplayData.CanSelectCards = false;
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetStateType), targetStateType, null);
            }
        }
        #endregion

        #region Public Methods
        public void EndTurn()
        {
            CurrentCombatStateType = CombatStateType.EnemyTurn;
        }

        IEnumerator EndTurnCalculation()
        {
            while (playerActionQueue.Count > 0)
            {
                var playerAction = playerActionQueue.Dequeue();
                if (playerAction.Key == CardActionType.EarnMana)
                {
                    playerFocusActionQueue.Enqueue(playerAction.Value);
                }else if (playerAction.Key == CardActionType.Attack)
                {
                    playerAttackActionQueue.Enqueue(playerAction.Value);
                }
                else
                {
                    playerPrepareActionQueue.Enqueue(playerAction.Value);
                }
            }
            
            while (enemyActionQueue.Count > 0)
            {
                var enemyAction = enemyActionQueue.Dequeue();
                if (enemyAction.Key == EnemyIntentionType.Focus)
                {
                    enemyFocusActionQueue.Enqueue(enemyAction.Value);
                }else if (enemyAction.Key == EnemyIntentionType.Attack)
                {
                    enemyAttackActionQueue.Enqueue(enemyAction.Value);
                }
                else
                {
                    enemyPrepareActionQueue.Enqueue(enemyAction.Value);
                }
            }

            yield return ExecutePlayerAndEnemyActionQueues(playerFocusActionQueue, enemyFocusActionQueue);
            yield return ExecutePlayerAndEnemyActionQueues(playerPrepareActionQueue, enemyPrepareActionQueue);
            yield return ExecutePlayerAndEnemyActionQueues(playerAttackActionQueue, enemyAttackActionQueue);
        }

        IEnumerator ExecutePlayerAndEnemyActionQueues(Queue<IEnumerator> playerQueue, Queue<IEnumerator> enemyQueue)
        {
            List<IEnumerator> focusActions = new List<IEnumerator>();
            focusActions.Add(ExecuteActionQueue(playerQueue, null));
            focusActions.Add(ExecuteActionQueue(enemyQueue, null));
            yield return StartCoroutine(WaitForAll(focusActions));
        }
        
        private IEnumerator WaitForAll(List<IEnumerator> coroutines)
        {
            int tally = 0;

            foreach(IEnumerator c in coroutines)
            {
                StartCoroutine(RunCoroutine(c));
            }

            while (tally > 0)
            {
                yield return null;
            }

            IEnumerator RunCoroutine(IEnumerator c)
            {
                tally++;
                yield return StartCoroutine(c);
                tally--;
            }
        }

        private IEnumerator ExecutePlayerActionQueue(Action callback)
        {
            yield return StartCoroutine(ExecuteNextActionInPlayerActionQueue());
            callback?.Invoke();
        }
        
        IEnumerator ExecuteNextActionInPlayerActionQueue()
        {
            if (PlayerActionQueue.Count > 0)
            {
                yield return StartCoroutine(PlayerActionQueue.Dequeue().Value);
            }
            else
            {
                yield break;
            }
            StartCoroutine(ExecuteNextActionInPlayerActionQueue());
        }
        
        private IEnumerator ExecutePEnemyActionQueue(Action callback)
        {
            yield return StartCoroutine(ExecuteNextActionInEnemyActionQueue());
            callback?.Invoke();
        }
        
        IEnumerator ExecuteNextActionInEnemyActionQueue()
        {
            if (EnemyActionQueue.Count > 0)
            {
                yield return StartCoroutine(EnemyActionQueue.Dequeue().Value);
            }
            else
            {
                yield break;
            }
            StartCoroutine(ExecuteNextActionInEnemyActionQueue());
        }
        
        private IEnumerator ExecuteActionQueue(Queue<IEnumerator> actionQueue, Action callback)
        {
            yield return StartCoroutine(ExecuteNextInActionQueue(actionQueue));
            callback?.Invoke();
        }
        
        IEnumerator ExecuteNextInActionQueue(Queue<IEnumerator> actionQueue)
        {
            if (actionQueue.Count > 0)
            {
                yield return StartCoroutine(actionQueue.Dequeue());
            }
            else
            {
                yield break;
            }
            StartCoroutine(ExecuteNextInActionQueue(actionQueue));
        }
        
        public void OnAllyDeath(AllyBase targetAlly)
        {
            var targetAllyData = NueGameManager.PersistentGameplayData.AllyList.Find(x =>
                x.AllyCharacterData.CharacterID == targetAlly.AllyCharacterData.CharacterID);
            if (NueGameManager.PersistentGameplayData.AllyList.Count>1)
                NueGameManager.PersistentGameplayData.AllyList.Remove(targetAllyData);
            CurrentAlliesList.Remove(targetAlly);
            NueUIManager.InformationCanvas.ResetCanvas();
            if (CurrentAlliesList.Count<=0)
                LoseCombat();
        }
        public void OnEnemyDeath(EnemyBase targetEnemy)
        {
            CurrentEnemiesList.Remove(targetEnemy);
            if (CurrentEnemiesList.Count<=0)
                WinCombat();
        }
        public void DeactivateCardHighlights()
        {
            foreach (var currentEnemy in CurrentEnemiesList)
                currentEnemy.EnemyCanvas.SetHighlight(false);

            foreach (var currentAlly in CurrentAlliesList)
                currentAlly.AllyCanvas.SetHighlight(false);
        }
        public void IncreaseMana(int target)
        {
            NueGameManager.PersistentGameplayData.CurrentMana += target;
            NueUIManager.CombatCanvas.SetPileTexts();
        }
        public void HighlightCardTarget(ActionTargetType targetTypeTargetType)
        {
            switch (targetTypeTargetType)
            {
                case ActionTargetType.Enemy:
                    foreach (var currentEnemy in CurrentEnemiesList)
                        currentEnemy.EnemyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.Ally:
                    foreach (var currentAlly in CurrentAlliesList)
                        currentAlly.AllyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.AllEnemies:
                    foreach (var currentEnemy in CurrentEnemiesList)
                        currentEnemy.EnemyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.AllAllies:
                    foreach (var currentAlly in CurrentAlliesList)
                        currentAlly.AllyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.RandomEnemy:
                    foreach (var currentEnemy in CurrentEnemiesList)
                        currentEnemy.EnemyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.RandomAlly:
                    foreach (var currentAlly in CurrentAlliesList)
                        currentAlly.AllyCanvas.SetHighlight(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetTypeTargetType), targetTypeTargetType, null);
            }
        }
        #endregion
        
        #region Private Methods
        private void BuildEnemies()
        {
            CurrentEncounter = NueGameManager.EncounterData.GetEnemyEncounter(
                NueGameManager.PersistentGameplayData.CurrentStageId,
                NueGameManager.PersistentGameplayData.CurrentEncounterId,
                NueGameManager.PersistentGameplayData.IsFinalEncounter);
            
            var enemyList = CurrentEncounter.EnemyList;
            for (var i = 0; i < enemyList.Count; i++)
            {
                var clone = Instantiate(enemyList[i].EnemyPrefab, EnemyPosList.Count >= i ? EnemyPosList[i] : EnemyPosList[0]);
                clone.BuildCharacter();
                CurrentEnemiesList.Add(clone);
            }
        }
        private void BuildAllies()
        {
            for (var i = 0; i < NueGameManager.PersistentGameplayData.AllyList.Count; i++)
            {
                var clone = Instantiate(NueGameManager.PersistentGameplayData.AllyList[i], AllyPosList.Count >= i ? AllyPosList[i] : AllyPosList[0]);
                clone.BuildCharacter();
                CurrentAlliesList.Add(clone);
            }
        }
        private void LoseCombat()
        {
            if (CurrentCombatStateType == CombatStateType.EndCombat) return;
            
            CurrentCombatStateType = CombatStateType.EndCombat;
            
            CollectionManager.DiscardHand();
            CollectionManager.DiscardPile.Clear();
            CollectionManager.DrawPile.Clear();
            CollectionManager.HandPile.Clear();
            CollectionManager.HandController.hand.Clear();
            NueUIManager.CombatCanvas.gameObject.SetActive(true);
            NueUIManager.CombatCanvas.CombatLosePanel.SetActive(true);
        }
        private void WinCombat()
        {
            if (CurrentCombatStateType == CombatStateType.EndCombat) return;
          
            CurrentCombatStateType = CombatStateType.EndCombat;
           
            foreach (var allyBase in CurrentAlliesList)
            {
                NueGameManager.PersistentGameplayData.SetAllyHealthData(allyBase.AllyCharacterData.CharacterID,
                    allyBase.CharacterStats.CurrentHealth, allyBase.CharacterStats.MaxHealth);
            }
            
            CollectionManager.ClearPiles();
            
           
            if (NueGameManager.PersistentGameplayData.IsFinalEncounter)
            {
                NueUIManager.CombatCanvas.CombatWinPanel.SetActive(true);
            }
            else
            {
                CurrentMainAlly.CharacterStats.ClearAllStatus();
                NueGameManager.PersistentGameplayData.CurrentEncounterId++;
                NueUIManager.CombatCanvas.gameObject.SetActive(false);
                NueUIManager.RewardCanvas.gameObject.SetActive(true);
                NueUIManager.RewardCanvas.PrepareCanvas();
                NueUIManager.RewardCanvas.BuildReward(RewardType.Gold);
                NueUIManager.RewardCanvas.BuildReward(RewardType.Card);
            }
           
        }
        #endregion
        
        #region Routines
        private IEnumerator EnemyTurnRoutine()
        {
            var waitDelay = new WaitForSeconds(0.1f);

            foreach (var currentEnemy in CurrentEnemiesList)
            {
                yield return currentEnemy.StartCoroutine(nameof(EnemyExample.ActionRoutine));
                yield return waitDelay;
            }

#if ACTION_QUEUE
            StartCoroutine(EndTurnCalculation());
#endif

            if (CurrentCombatStateType != CombatStateType.EndCombat)
                CurrentCombatStateType = CombatStateType.AllyTurn;
        }
        #endregion
    }
}