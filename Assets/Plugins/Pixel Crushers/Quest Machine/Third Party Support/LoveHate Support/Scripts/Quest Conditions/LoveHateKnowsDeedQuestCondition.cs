// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using PixelCrushers.LoveHate;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    public class LoveHateKnowsDeedQuestCondition : QuestCondition
    {

        [Tooltip("GameObject name or ID of Faction Member whose memory to check.")]
        [SerializeField]
        private StringField m_factionMemberID;

        [Tooltip("Actor who committed deed.")]
        [SerializeField]
        private int m_actorID;

        [Tooltip("Target deed was done to.")]
        [SerializeField]
        private int m_targetID;

        [Tooltip("Deed tag.")]
        [SerializeField]
        private StringField m_deedTag;

        [Tooltip("If ticked and Faction Member already knows deed, become true immediately. If unticked, Faction Member must learn deed after condition checking has started.")]
        [SerializeField]
        private bool m_acceptAlreadyKnown;

        public StringField factionMemberID
        {
            get { return m_factionMemberID; }
            set { m_factionMemberID = value; }
        }

        public int actorID
        {
            get { return m_actorID; }
            set { m_actorID = value; }
        }

        public int targetID
        {
            get { return m_targetID; }
            set { m_targetID = value; }
        }

        public StringField deedTag
        {
            get { return m_deedTag; }
            set { m_deedTag = value; }
        }

        public bool acceptAlreadyKnown
        {
            get { return m_acceptAlreadyKnown; }
            set { m_acceptAlreadyKnown = value; }
        }

        private FactionMember m_factionMember = null;
        private RememberDeedNotifier m_rememberDeedNotifier = null;

        public override void StartChecking(System.Action trueAction)
        {
            base.StartChecking(trueAction);
            m_factionMember = LoveHateUtility.FindFactionMember(factionMemberID, quest.questGiverID);
            if (m_factionMember == null) return;
            if (acceptAlreadyKnown && m_factionMember.KnowsAboutDeed(actorID, targetID, StringField.GetStringValue(deedTag)))
            {
                SetTrue();
                return;
            }
            m_rememberDeedNotifier = m_factionMember.gameObject.AddComponent<RememberDeedNotifier>();
            m_rememberDeedNotifier.rememberedDeed += OnRememberedDeed;
        }

        public override void StopChecking()
        {
            if (m_rememberDeedNotifier != null) Destroy(m_rememberDeedNotifier);
            m_rememberDeedNotifier = null;
            base.StopChecking();
        }

        private void OnRememberedDeed(Rumor rumor)
        {
            if (rumor == null) return;
            if (string.Equals(rumor.tag, StringField.GetStringValue(deedTag)) &&
                ((rumor.actorFactionID == actorID) || LoveHateUtility.factionDatabase.FactionHasAncestor(rumor.actorFactionID, actorID)) &&
                ((rumor.targetFactionID == targetID) || LoveHateUtility.factionDatabase.FactionHasAncestor(rumor.targetFactionID, targetID)))
            {
                if (m_rememberDeedNotifier != null) Destroy(m_rememberDeedNotifier);
                m_rememberDeedNotifier = null;
                SetTrue();
            }
        }

        public override string GetEditorName()
        {
            var knower = StringField.IsNullOrEmpty(factionMemberID) ? "Quest Giver" : StringField.GetStringValue(factionMemberID);
            return knower + " knows " + LoveHateUtility.GetFactionName(actorID) + " " + deedTag + " " + LoveHateUtility.GetFactionName(targetID);
        }

    }

}
