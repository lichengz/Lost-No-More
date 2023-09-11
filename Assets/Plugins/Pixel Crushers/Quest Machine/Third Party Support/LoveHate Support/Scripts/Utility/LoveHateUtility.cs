// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using PixelCrushers.LoveHate;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// Utility methods for integrating Quest Machine and Love/Hate.
    /// </summary>
    public static class LoveHateUtility
    {

        public enum ChangeMode { Set, Increment}

        public enum ComparisonMode { AtLeast, AtMost }

        private static FactionManager m_factionManager = null;
        public static FactionManager factionManager
        {
            get
            {
                if (m_factionManager == null) m_factionManager = GameObject.FindObjectOfType<FactionManager>();
                return m_factionManager;
            }
        }

        public static FactionDatabase factionDatabase
        {
            get { return (factionManager != null) ? factionManager.factionDatabase : null; }
        }

        private static string[] m_factionNames = null;
        public static string[] factionNames
        {
            get
            {
                if (m_factionNames == null) m_factionNames = GetFactionNames();
                return m_factionNames;
            }
        }

        private static string[] m_relationshipNames = null;
        public static string[] relationshipNames
        {
            get
            {
                if (m_relationshipNames == null) m_relationshipNames = GetRelationshipNames();
                return m_relationshipNames;
            }
        }

        private static string[] GetFactionNames()
        {
            if (factionManager == null || factionManager.factionDatabase == null) return null;
            var result = new string[factionManager.factionDatabase.factions.Length];
            for (int i = 0; i < factionManager.factionDatabase.factions.Length; i++)
            {
                result[i] = factionManager.factionDatabase.factions[i].name;
            }
            return result;
        }

        public static string GetFactionName(int factionID)
        {
            if (factionManager != null)
            {
                var faction = factionManager.GetFactionSilent(factionID);
                if (faction != null) return faction.name;
            }
            return "Faction ID=" + factionID;
        }

        public static int GetFactionID(string factionName, StringField alternateFactionName)
        {
            return GetFactionID(factionName, StringField.GetStringValue(alternateFactionName));
        }

        public static int GetFactionID(StringField factionName, StringField alternateFactionName)
        {
            return GetFactionID(StringField.GetStringValue(factionName), StringField.GetStringValue(alternateFactionName));
        }

        public static int GetFactionID(string factionName, string alternateFactionName)
        {
            if (factionDatabase != null)
            {
                var faction = factionDatabase.GetFaction(factionName);
                if (faction != null) return faction.id;
                faction = factionDatabase.GetFaction(alternateFactionName);
                if (faction != null) return faction.id;
            }
            return -1;
        }

        private static string[] GetRelationshipNames()
        {
            if (factionManager == null || factionManager.factionDatabase == null) return null;
            var result = new string[factionManager.factionDatabase.relationshipTraitDefinitions.Length];
            for (int i = 0; i < factionManager.factionDatabase.relationshipTraitDefinitions.Length; i++)
            {
                result[i] = factionManager.factionDatabase.relationshipTraitDefinitions[i].name;
            }
            return result;
        }

        public static string GetRelationshipName(int relationshipID)
        {
            if (relationshipNames != null && 0 <= relationshipID && relationshipID < relationshipNames.Length)
            {
                return relationshipNames[relationshipID];
            }
            return (relationshipID == 0) ? "Affinity" : ("Relationship ID=" + relationshipID);
        }

        public static FactionMember FindFactionMember(StringField id, StringField defaultID = null)
        {
            return FindFactionMember(StringField.GetStringValue(id), StringField.GetStringValue(defaultID));
        }

        public static FactionMember FindFactionMember(string id, StringField defaultID = null)
        {
            return FindFactionMember(id, StringField.GetStringValue(defaultID));
        }

        public static FactionMember FindFactionMember(string id, string defaultID = null)
        {
            if (string.IsNullOrEmpty(id)) id = defaultID;
            var go = GameObject.Find(id);
            var member = (go != null) ? go.GetComponentInChildren<FactionMember>() : null;
            if (member != null) return member;
            member = FindFactionMemberInType<IdentifiableQuestListContainer>(id);
            if (member != null) return member;
            member = FindFactionMemberInType<QuestEntity>(id);
            if (member != null) return member;
            return null;
        }

        private static FactionMember FindFactionMemberInType<T>(string id) where T : MonoBehaviour
        {
            var components = GameObject.FindObjectsOfType<T>();
            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];
                if (StringField.Equals(QuestMachineMessages.GetID(component), id))
                {
                    var member = components[i].GetComponentInChildren<FactionMember>();
                    if (member != null) return member;
                }
            }
            return null;
        }

    }

}
