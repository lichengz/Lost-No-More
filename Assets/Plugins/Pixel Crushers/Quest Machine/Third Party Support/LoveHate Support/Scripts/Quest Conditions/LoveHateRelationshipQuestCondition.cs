// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using System.Collections;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    public class LoveHateRelationshipQuestCondition : QuestCondition
    {

        [SerializeField]
        private int m_judgeFactionID = -1;

        [SerializeField]
        private int m_subjectFactionID = -1;

        [SerializeField]
        private int m_relationshipID = 0;

        [Tooltip("How to check relationship value.")]
        [SerializeField]
        private LoveHateUtility.ComparisonMode m_mode = LoveHateUtility.ComparisonMode.AtLeast;

        [Tooltip("Required value.")]
        [SerializeField]
        private float m_value = 0;

        [Tooltip("Frequency in seconds to check memory.")]
        [SerializeField]
        private float m_frequencyToCheck = 2;

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

        public LoveHateUtility.ComparisonMode mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        public float value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public float frequencyToCheck
        {
            get { return m_frequencyToCheck; }
            set { m_frequencyToCheck = value; }
        }

        private Coroutine m_coroutine = null;

        public override void StartChecking(System.Action trueAction)
        {
            base.StartChecking(trueAction);
            StartPolling();
        }

        public override void StopChecking()
        {
            StopPolling();
            base.StopChecking();
        }

        private void StartPolling()
        {
            if (LoveHateUtility.factionManager == null) return;
            m_coroutine = LoveHateUtility.factionManager.StartCoroutine(PollRelationship());
        }

        private void StopPolling()
        {
            if (m_coroutine != null && LoveHateUtility.factionManager != null) LoveHateUtility.factionManager.StopCoroutine(m_coroutine);
            m_coroutine = null;
        }

        private IEnumerator PollRelationship()
        {
            var delay = new WaitForSeconds(frequencyToCheck);
            while (true)
            {
                if (IsConditionMet())
                {
                    StopPolling();
                    SetTrue();
                    yield break;
                }
                else
                {
                    yield return delay;
                }
            }
        }

        private bool IsConditionMet()
        {
            var currentValue = LoveHateUtility.factionDatabase.GetRelationshipTrait(judgeFactionID, subjectFactionID, relationshipID);
            switch (mode)
            {
                case LoveHateUtility.ComparisonMode.AtLeast:
                    return currentValue >= value;
                case LoveHateUtility.ComparisonMode.AtMost:
                    return currentValue <= value;
            }
            return false;
        }

        public override string GetEditorName()
        {
            if (judgeFactionID == -1 || subjectFactionID == -1) return base.GetEditorName();
            return LoveHateUtility.GetFactionName(judgeFactionID) + "->" + LoveHateUtility.GetFactionName(subjectFactionID) + " " +
                LoveHateUtility.GetRelationshipName(relationshipID) + " " + mode + " " + value;
        }

    }

}
