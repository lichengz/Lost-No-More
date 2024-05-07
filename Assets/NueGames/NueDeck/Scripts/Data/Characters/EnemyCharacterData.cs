using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.NueExtentions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.NueDeck.Scripts.Data.Characters
{
    [CreateAssetMenu(fileName = "Enemy Character Data",menuName = "NueDeck/Characters/Enemy",order = 1)]
    public class EnemyCharacterData : CharacterDataBase
    {
        [Header("Enemy Defaults")] 
        [SerializeField] private EnemyBase enemyPrefab;
        [SerializeField] private bool followAbilityPattern;
        [SerializeField] private List<EnemyAbilityData> enemyAbilityList;
        public List<EnemyAbilityData> EnemyAbilityList => enemyAbilityList;
        [SerializeField] private List<EnemyFocusData> enemyFocusList;
        public List<EnemyFocusData> EnemyFocusList => enemyFocusList;

        public EnemyBase EnemyPrefab => enemyPrefab;

        public EnemyAbilityData GetAbility()
        {
            return EnemyAbilityList.RandomItem();
        }
        
        public EnemyAbilityData GetAbility(int usedAbilityCount)
        {
            if (followAbilityPattern)
            {
                var index = usedAbilityCount % EnemyAbilityList.Count;
                return EnemyAbilityList[index];
            }

            return GetAbility();
        }
        
        public EnemyAbilityData GetAbility(List<KeyValuePair<EnemyAbilityData, float>> abilityList)
        {
            List<float> possibilityTiers = new List<float>();
            float random = 0;
            possibilityTiers.Add(0);
            for(int i = 0; i < abilityList.Count - 1; i++)
            {
                random += abilityList[i].Value;
                possibilityTiers.Add(random);
            }

            float nextAbilityRandom = Random.Range(0, 100);
            int nextAbilityIndex = 0;
            for (int i = 0; i < possibilityTiers.Count; i++)
            {
                if (nextAbilityRandom > possibilityTiers[i])
                {
                    nextAbilityIndex = i;
                }
                else
                {
                    break;
                }
            }
            return abilityList[nextAbilityIndex].Key;
        }
        
        private bool IsNeedFocus(CharacterStats characterStats)
        {
            return characterStats.CurrentFocus < characterStats.MaxFocus;
        }

        public List<KeyValuePair<EnemyAbilityData, float>> GetPossibleAbilitiesList(CharacterStats characterStats)
        {
            float currentFocus = characterStats.CurrentFocus;

            List<KeyValuePair<EnemyAbilityData, float>> list = new List<KeyValuePair<EnemyAbilityData, float>>();
            List<EnemyAbilityData> enemyAbilityWithEnoughFocus = new List<EnemyAbilityData>();
            foreach (var ability in EnemyAbilityList)
            {
                if (ability.FocusCost <= currentFocus)
                {
                    enemyAbilityWithEnoughFocus.Add(ability);
                }
            }
            int abilityCount = enemyAbilityWithEnoughFocus.Count;
            float ranTotal = 0;
            List<float> tmpRan = new List<float>();
            
            // Focus Possibility
            bool isNeedFocus = IsNeedFocus(characterStats);
            float focusPossibility = 0;
            if (isNeedFocus)
            {
                focusPossibility = characterStats.CurrentFocus == 0 ? 100 : Random.Range(0, 100);
                ranTotal += focusPossibility;
            }

            // Ability Possibility
            for(int i = 0; i < abilityCount; i++)
            {
                float possibility = Random.Range(0, 100);
                ranTotal += possibility;
                tmpRan.Add(possibility);
            }
            
            // Add Focus and Ability to result list
            float curPossibility = tmpRan[0] / ranTotal * 100;
            
            // Focus
            if (characterStats.CurrentFocus == 0)
            {
                // must focus since current focus is 0
                list.Add(new KeyValuePair<EnemyAbilityData, float>(enemyFocusList[0], focusPossibility));
                return list;
            }
            
            if(isNeedFocus)
            {
                list.Add(new KeyValuePair<EnemyAbilityData, float>(enemyFocusList[0], focusPossibility / ranTotal * 100));
            }
            
            for (int i = 0; i < abilityCount; i++)
            {
                curPossibility = tmpRan[i] / ranTotal * 100;

                // Ability
                list.Add(new KeyValuePair<EnemyAbilityData, float>(enemyAbilityWithEnoughFocus[i], curPossibility));
            }
            
            return list;
        }
    }
    
    [Serializable]
    public class EnemyAbilityData
    {
        [Header("Settings")]
        [SerializeField] private string name;
        [SerializeField] private EnemyIntentionData intention;
        [SerializeField] private bool hideActionValue;
        [SerializeField] private List<EnemyActionData> actionList;
        [SerializeField] private int focusCost;
        public string Name => name;
        public EnemyIntentionData Intention => intention;
        public List<EnemyActionData> ActionList => actionList;
        public int FocusCost => focusCost;
        public bool HideActionValue => hideActionValue;
    }
    
    [Serializable]
    public class EnemyActionData
    {
        [SerializeField] private EnemyActionType actionType;
        [SerializeField] private int minActionValue;
        [SerializeField] private int maxActionValue;
        public EnemyActionType ActionType => actionType;
        public int ActionValue => Random.Range(minActionValue,maxActionValue);
        
    }
    
    [Serializable]
    public class EnemyFocusData : EnemyAbilityData
    {
        
    }
    
    
    
}