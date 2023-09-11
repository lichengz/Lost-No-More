// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// This reward system increases a Love/Hate relationship value.
    /// </summary>
    public class LoveHateRelationshipRewardSystem : RewardSystem
    {

        [Tooltip("The faction whose relationship to update.")]
        [SerializeField]
        private int m_judgeFactionID;

        [Tooltip("The subject that the judge faction is judging.")]
        [SerializeField]
        private int m_subjectFactionID;

        [Tooltip("The relationship trait whose value to update.")]
        [SerializeField]
        private int m_relationshipID = 0;

        [Tooltip("Increment the relationship by the product of this value times the reward points.")]
        [SerializeField]
        private float m_multiplier = 1;

        [Tooltip("The reward text, where {0} is amount to change the relationship, {1} is relationship type, and {2} is faction whose relationship to update.")]
        [SerializeField]
        private StringField m_rewardText = new StringField("{0} {1} with {2}");

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

        public float multiplier
        {
            get { return m_multiplier; }
            set { m_multiplier = value; }
        }

        public StringField rewardText
        {
            get { return m_rewardText; }
            set { m_rewardText = value; }
        }

        // The quest generator will call this method to try to use up points
        // by choosing rewards to offer.
        public override int DetermineReward(int points, Quest quest)
        {
            var value = Mathf.RoundToInt(multiplier * points);

            // Add some UI content to the quest's offerContentList:
            var bodyText = BodyTextQuestContent.CreateInstance<BodyTextQuestContent>();
            bodyText.bodyText = new StringField(string.Format(StringField.GetStringValue(rewardText), 
                new object[] { value, LoveHateUtility.GetRelationshipName(relationshipID), LoveHateUtility.GetFactionName(judgeFactionID) }));
            quest.offerContentList.Add(bodyText);

            // Add an action to the quest's Successful state:
            var action = LoveHateChangeRelationshipQuestAction.CreateInstance<LoveHateChangeRelationshipQuestAction>();
            action.judgeFactionID = judgeFactionID;
            action.subjectFactionID = subjectFactionID;
            action.relationshipID = relationshipID;
            action.mode = LoveHateUtility.ChangeMode.Increment;
            action.value = value;
            quest.GetStateInfo(QuestState.Successful).actionList.Add(action);

            return points; // Doesn't use any points.
        }

    }
}
