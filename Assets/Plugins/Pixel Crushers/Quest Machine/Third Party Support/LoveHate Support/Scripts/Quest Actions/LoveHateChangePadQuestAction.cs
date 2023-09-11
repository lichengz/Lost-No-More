// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using System;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// This quest action modifies a faction member's PAD values.
    /// </summary>
    public class LoveHateChangePadQuestAction : QuestAction
    {

        [Tooltip("GameObject name or ID of Faction Member whose PAD to change.")]
        [SerializeField]
        private StringField m_factionMemberID;

        [Tooltip("How to modify PAD values.")]
        [SerializeField]
        private LoveHateUtility.ChangeMode m_mode = LoveHateUtility.ChangeMode.Increment;

        [SerializeField]
        private float m_happiness = 0;

        [SerializeField]
        private float m_pleasure = 0;

        [SerializeField]
        private float m_arousal = 0;

        [SerializeField]
        private float m_dominance = 0;

        public StringField factionMemberID
        {
            get { return m_factionMemberID; }
            set { m_factionMemberID = value; }
        }

        public LoveHateUtility.ChangeMode mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        public float happiness
        {
            get { return m_happiness; }
            set { m_happiness = value; }
        }

        public float pleasure
        {
            get { return m_pleasure; }
            set { m_pleasure = value; }
        }

        public float arousal
        {
            get { return m_arousal; }
            set { m_arousal = value; }
        }

        public float dominance
        {
            get { return m_dominance; }
            set { m_dominance = value; }
        }

        public override void Execute()
        {
            base.Execute();
            var member = LoveHateUtility.FindFactionMember(factionMemberID, quest.questGiverID);
            if (member == null)
            {
                if (Debug.isDebugBuild) Debug.LogWarning("Quest Machine: " + GetType().Name + " can't find Faction Member '" + factionMemberID + "' in the scene.");
            }
            else
            {
                if (QuestMachine.debug) Debug.Log("Quest Machine: " + GetEditorName());
                switch (mode)
                {
                    case LoveHateUtility.ChangeMode.Set:
                        member.ModifyPAD(member.pad.happiness - member.pad.happiness + happiness, 
                            member.pad.pleasure - member.pad.pleasure + pleasure,
                            member.pad.arousal - member.pad.arousal + arousal,
                            member.pad.dominance - member.pad.dominance + dominance);
                        break;
                    case LoveHateUtility.ChangeMode.Increment:
                        member.ModifyPAD(happiness, pleasure, arousal, dominance);
                        break;
                }
            }
        }

        public override string GetEditorName()
        {
            return mode + " " + factionMemberID + " PAD(" + pleasure + "," + arousal + "," + dominance + ") Happiness=" + happiness;
        }

    }

}
