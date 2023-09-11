// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using PixelCrushers.LoveHate;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// This quest action modifies a Love/Hate relationship value.
    /// </summary>
    public class LoveHateChangeRelationshipQuestAction : QuestAction
    {

        [SerializeField]
        private int m_judgeFactionID = -1;

        [SerializeField]
        private int m_subjectFactionID = -1;

        [SerializeField]
        private int m_relationshipID = 0;

        [Tooltip("How to modify relationship value.")]
        [SerializeField]
        private LoveHateUtility.ChangeMode m_mode = LoveHateUtility.ChangeMode.Increment;

        [Tooltip("Value to set/change relationship.")]
        [SerializeField]
        private float m_value = 0;

        public int judgeFactionID
        {
            get { return m_judgeFactionID; }
            set { m_judgeFactionID = value; }
        }

        public int subjectFactionID
        {
            get { return m_subjectFactionID; }
            set { m_subjectFactionID = value; }
        }

        public int relationshipID
        {
            get { return m_relationshipID; }
            set { m_relationshipID = value; }
        }

        public LoveHateUtility.ChangeMode mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        public float value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public override void Execute()
        {
            base.Execute();
            if (LoveHateUtility.factionManager == null)
            {
                if (Debug.isDebugBuild) Debug.LogWarning("Quest Machine: " + GetType().Name + " can't find a Faction Manager in the scene.");
            }
            else
            {
                if (QuestMachine.debug) Debug.Log("Quest Machine: " + GetEditorName());
                switch (mode)
                {
                    case LoveHateUtility.ChangeMode.Set:
                        LoveHateUtility.factionManager.factionDatabase.SetPersonalRelationshipTrait(judgeFactionID, subjectFactionID, relationshipID, value);
                        break;
                    case LoveHateUtility.ChangeMode.Increment:
                        LoveHateUtility.factionManager.factionDatabase.ModifyPersonalRelationshipTrait(judgeFactionID, subjectFactionID, relationshipID, value);
                        break;
                }
            }
        }

        public override string GetEditorName()
        {
            if (judgeFactionID == -1 || subjectFactionID == -1) return base.GetEditorName();
            return mode + " " + LoveHateUtility.GetFactionName(judgeFactionID) + "->" + LoveHateUtility.GetFactionName(subjectFactionID) + " " +
                LoveHateUtility.GetRelationshipName(relationshipID) + " " + value;
        }

    }

}
