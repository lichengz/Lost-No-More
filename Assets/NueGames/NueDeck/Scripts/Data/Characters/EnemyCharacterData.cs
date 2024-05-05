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

        public List<KeyValuePair<EnemyAbilityData, float>> GetPossibleAbilitiesList(CharacterStats characterStats)
        {
            float currentFocus = characterStats.CurrentFocus;

            List<KeyValuePair<EnemyAbilityData, float>> list = new List<KeyValuePair<EnemyAbilityData, float>>();
            int count = EnemyAbilityList.Count;
            float ranTotal = 0;
            List<float> tmpRan = new List<float>();
            
            // Ability
            for(int i = 0; i < count; i++)
            {
                float possibility = Random.Range(0, 100);
                ranTotal += possibility;
                tmpRan.Add(possibility);
            }
            
            // Focus
            if (currentFocus == 0)
            {
                float possibility = Random.Range(0, 100);
                ranTotal += possibility;
                tmpRan.Add(possibility);
            }

            for (int i = 0; i < tmpRan.Count; i++)
            {
                float curPossibility = tmpRan[i] / ranTotal * 100;
                if (i > enemyAbilityList.Count - 1)
                {
                    // Focus
                    list.Add(new KeyValuePair<EnemyAbilityData, float>(enemyFocusList[0], curPossibility));
                }
                else
                {
                    // Ability
                    list.Add(new KeyValuePair<EnemyAbilityData, float>(enemyAbilityList[i], curPossibility));
                }
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
        [SerializeField] private float focusCost;
        public string Name => name;
        public EnemyIntentionData Intention => intention;
        public List<EnemyActionData> ActionList => actionList;
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