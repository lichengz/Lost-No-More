// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using PixelCrushers.LoveHate;
using System.Collections.Generic;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// Sets EntityType drive values to the personality trait values
    /// of their corresponding Love/Hate factions.
    /// </summary>
    public class LoveHateTraitsToDrives : MonoBehaviour
    {

        [Tooltip("Entity types to sync with Love/Hate factions.")]
        [SerializeField]
        private List<EntityType> m_affectedEntityTypes;

        public List<EntityType> affectedEntityTypes
        {
            get { return m_affectedEntityTypes; }
            protected set { m_affectedEntityTypes = value; }
        }

        protected virtual void Start()
        {
            if (FactionManager.instance == null)
            {
                if (Debug.isDebugBuild) Debug.LogError(GetType().Name + ": Can't initialize. The scene has no Faction Manager.", this);
            }
            else if (FactionManager.instance.factionDatabase == null)
            {
                if (Debug.isDebugBuild) Debug.LogError(GetType().Name + ": Can't initialize. No faction database is assigned to the Faction Manager.", this);
            }
            else
            {
                ApplyLoveHateTraitsToEntityTypes();
            }
        }

        protected virtual void OnEnable()
        {
            if (FactionManager.instance == null || FactionManager.instance.factionDatabase == null) return;
            FactionManager.instance.factionDatabase.personalityTraitChanged += OnPersonalityTraitChanged;
        }

        protected virtual void OnDisable()
        {
            if (FactionManager.instance == null || FactionManager.instance.factionDatabase == null) return;
            FactionManager.instance.factionDatabase.personalityTraitChanged -= OnPersonalityTraitChanged;
        }

        protected virtual void OnPersonalityTraitChanged(int factionID, int traitID, float value)
        {
            // Copy current personality traits to entityType that matches faction:
            var factionDatabase = FactionManager.instance.factionDatabase;
            if (factionDatabase == null) return;
            var faction = factionDatabase.GetFaction(factionID);
            if (faction == null) return;
            var entityType = affectedEntityTypes.Find(x => string.Equals(x.name, faction.name));
            if (entityType == null) return;
            ApplyLoveHateTraitsToEntityType(entityType);
        }

        public void ApplyLoveHateTraitsToEntityTypes()
        {
            // Copy personality traits to all entityTypes:
            if (affectedEntityTypes == null) return;
            for (int i = 0; i < affectedEntityTypes.Count; i++)
            {
                ApplyLoveHateTraitsToEntityType(affectedEntityTypes[i]);
            }
        }

        public void ApplyLoveHateTraitsToEntityType(EntityType entityType)
        {
            if (entityType == null) return;
            var factionDatabase = FactionManager.instance.factionDatabase;
            for (int i = 0; i < entityType.driveValues.Count; i++)
            {
                var driveValue = entityType.driveValues[i];
                if (driveValue == null || driveValue.drive == null) continue;
                var faction = factionDatabase.GetFaction(entityType.name);
                if (faction == null) continue;
                for (int j = 0; j < factionDatabase.personalityTraitDefinitions.Length; j++)
                {
                    var def = factionDatabase.personalityTraitDefinitions[j];
                    if (string.Equals(driveValue.drive.name, def.name) && j < faction.traits.Length)
                    {
                        driveValue.value = faction.traits[j];
                        break;
                    }
                }
            }
        }
    }
}
