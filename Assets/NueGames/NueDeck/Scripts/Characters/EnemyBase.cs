using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Characters;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.EnemyBehaviour;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Interfaces;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.NueExtentions;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Characters
{
    public class EnemyBase : CharacterBase, IEnemy
    {
        [Header("Enemy Base References")]
        [SerializeField] protected EnemyCharacterData enemyCharacterData;
        [SerializeField] protected EnemyCanvas enemyCanvas;
        [SerializeField] protected SoundProfileData deathSoundProfileData;
        [SerializeField] protected VoidEventChannelSO startCombatEvent;
        [SerializeField] protected IntEventChannelSO _closeUIDialogueEvent;

        protected EnemyAbilityData NextAbility;
        
        public EnemyCharacterData EnemyCharacterData => enemyCharacterData;
        public EnemyCanvas EnemyCanvas => enemyCanvas;
        public SoundProfileData DeathSoundProfileData => deathSoundProfileData;

        #region Setup
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            EnemyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(EnemyCharacterData,EnemyCanvas);
            CharacterStats.OnDeath += OnDeath;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
            CharacterStats.SetCurrentFocus(CharacterStats.CurrentFocus);
            CombatManager.OnAllyTurnStarted += ShowNextAbility;
            CombatManager.OnEnemyTurnStarted += CharacterStats.TriggerAllStatus;
            OnHurtEvent.OnEventRaised += UpdateDialogueLua;
        }
        protected override void OnDeath()
        {
            base.OnDeath();
            CombatManager.OnAllyTurnStarted -= ShowNextAbility;
            CombatManager.OnEnemyTurnStarted -= CharacterStats.TriggerAllStatus;
           
            CombatManager.OnEnemyDeath(this);
            NueAudioManager.PlayOneShot(DeathSoundProfileData.GetRandomClip());
            Destroy(gameObject);
            OnHurtEvent.OnEventRaised -= UpdateDialogueLua;
        }
        #endregion
        
        #region Private Methods

        private void UpdateDialogueLua()
        {
            GetComponent<BarkOnIdle>().StartBarkLoop();
            DialogueLua.SetVariable("EnemyIsHurt", true);
        }

        private int _usedAbilityCount;
        private void ShowNextAbility()
        {
            List<KeyValuePair<EnemyAbilityData, float>> abilityList = EnemyCharacterData.GetPossibleAbilitiesList(CharacterStats);
            DisplayPossibleIntents(abilityList);
            // NextAbility = EnemyCharacterData.GetAbility(_usedAbilityCount);
            NextAbility = EnemyCharacterData.GetAbility(abilityList);
            EnemyCanvas.IntentImage.sprite = NextAbility.Intention.IntentionSprite;


            if (NextAbility.HideActionValue)
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(false);
            }
            else
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(true);
                EnemyCanvas.NextActionValueText.text = NextAbility.ActionList[0].ActionValue.ToString();
            }

            _usedAbilityCount++;
            EnemyCanvas.IntentImage.gameObject.SetActive(false);
            EnemyCanvas.PossibleIntentsRoot.gameObject.SetActive(true);
        }

        private void DisplayPossibleIntents(List<KeyValuePair<EnemyAbilityData, float>> list)
        {
            Transform intentsRoot = enemyCanvas.PossibleIntentsRoot;
            Assert.IsTrue(intentsRoot.childCount > 0);
            Transform intentPrefab = intentsRoot.GetChild(0);
            int expectedCount = list.Count;
            int currentCount = intentsRoot.childCount;
            // spawn more if need more
            if (currentCount < expectedCount)
            {
                for (int i = currentCount; i < expectedCount; i++)
                {
                    Instantiate(intentPrefab, Vector3.zero, Quaternion.identity, intentsRoot);
                }
            }
            // setup what are needed
            for (int i = 0; i < expectedCount; i++)
            {
                KeyValuePair<EnemyAbilityData, float> pair = list[i];
                Transform intent = intentsRoot.GetChild(i);
                intent.GetComponent<Image>().sprite = pair.Key.Intention.IntentionSprite;
                intent.GetComponentInChildren<TextMeshProUGUI>().text = pair.Value.ToString("0.##\\%");
            }
            // destroy what are not needed
            if (currentCount > expectedCount)
            {
                for (int i = expectedCount; i < currentCount; i++)
                {
                    Destroy(intentsRoot.GetChild(i).gameObject);
                }
            }
        }
        #endregion
        
        #region Action Routines
        public virtual IEnumerator ActionRoutine()
        {
            if (CharacterStats.IsStunned)
                yield break;
            
            EnemyCanvas.IntentImage.gameObject.SetActive(true);
            EnemyCanvas.PossibleIntentsRoot.gameObject.SetActive(false);
            if (NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Attack || NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Debuff)
            {
                yield return StartCoroutine(AttackRoutine(NextAbility));
            }
            else
            {
                yield return StartCoroutine(BuffRoutine(NextAbility));
            }
        }
        
        protected virtual IEnumerator AttackRoutine(EnemyAbilityData targetAbility)
        {
            var waitFrame = new WaitForEndOfFrame();

            if (CombatManager == null) yield break;
            
            var target = CombatManager.CurrentAlliesList.RandomItem();
            
            var startPos = transform.position;
            var endPos = target.transform.position;

            var startRot = transform.localRotation;
            var endRot = Quaternion.Euler(60, 0, 60);

            CharacterStats.SpendFocus(targetAbility.FocusCost);
            
            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, startPos, endPos, startRot, endRot, 5));
          
            targetAbility.ActionList.ForEach(x=>EnemyActionProcessor.GetAction(x.ActionType).DoAction(new EnemyActionParameters(x.ActionValue,target,this)));
            
            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, endPos, startPos, endRot, startRot, 5));
        }
        
        protected virtual IEnumerator BuffRoutine(EnemyAbilityData targetAbility)
        {
            var waitFrame = new WaitForEndOfFrame();
            
            var target = CombatManager.CurrentEnemiesList.RandomItem();
            
            var startPos = transform.position;
            var endPos = startPos+new Vector3(0,0.2f,0);
            
            var startRot = transform.localRotation;
            var endRot = transform.localRotation;
            
            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, startPos, endPos, startRot, endRot, 5));
            
            targetAbility.ActionList.ForEach(x=>EnemyActionProcessor.GetAction(x.ActionType).DoAction(new EnemyActionParameters(x.ActionValue,target,this)));
            
            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, endPos, startPos, endRot, startRot, 5));
        }
        #endregion
        
        #region Other Routines
        private IEnumerator MoveToTargetRoutine(WaitForEndOfFrame waitFrame,Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float speed)
        {
            var timer = 0f;
            while (true)
            {
                timer += Time.deltaTime*speed;

                transform.position = Vector3.Lerp(startPos, endPos, timer);
                transform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }
        }

        #endregion
        
        #region Event Calling

        public void StartCombat()
        {
            _closeUIDialogueEvent.OnEventRaisedOnce(
                (int tmp) =>
                {
                    startCombatEvent.RaiseEvent();
                });
        }
        #endregion

    }
}