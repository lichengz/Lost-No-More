// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// Returns an urgency score based on the observer's relationship to the observed entity, 
    /// multiplied by a multiplier and a curve based on the entity count.
    /// </summary>
    [CreateAssetMenu(menuName = "Pixel Crushers/Quest Machine/Generator/Urgency Functions/Love\u2215Hate/Urgency By Relationship")]
    public class LoveHateRelationshipUrgencyFunction : UrgencyFunction
    {

        [SerializeField]
        private int m_relationshipID = 0;

        [Tooltip("Multiply relationship value by this (e.g., -1 to equate negative relationship to threat).")]
        [SerializeField]
        private float m_multiplier = 1;

        [Tooltip("Multiply the urgency by the value of this curve, where the keys indicate the number of entities the observer is aware of.")]
        [SerializeField]
        private AnimationCurve m_entityCountMultiplier = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1), new Keyframe(10, 10), new Keyframe(50, 20));

        /// <summary>
        /// Judge this relationship type.
        /// </summary>
        public int relationshipID
        {
            get { return m_relationshipID; }
            set { m_relationshipID = value; }
        }

        /// <summary>
        /// Multiply the relationship value by this multiplier.
        /// </summary>
        public float multiplier
        {
            get { return m_multiplier; }
            set { m_multiplier = value; }
        }

        /// <summary>
        /// The urgency value is multiplied by this curve, where the keys indicate the number of entities the observer is aware of.
        /// </summary>
        public AnimationCurve entityCountMultiplier
        {
            get { return m_entityCountMultiplier; }
            set { m_entityCountMultiplier = value; }
        }

        public override string typeName { get { return "By Relationship"; } }

        public override float Compute(WorldModel worldModel)
        {
            if (LoveHateUtility.factionDatabase == null) return 0;
            if (worldModel == null || worldModel.observer == null || worldModel.observer.entityType == null || worldModel.observed == null || worldModel.observed.entityType == null) return 0;
            var judgeID = LoveHateUtility.GetFactionID(worldModel.observer.entityType.name, worldModel.observer.entityType.displayName);
            var subjectID = LoveHateUtility.GetFactionID(worldModel.observer.entityType.name, worldModel.observer.entityType.displayName);
            var value = LoveHateUtility.factionDatabase.GetRelationshipTrait(judgeID, subjectID, relationshipID);
            value *= multiplier;
            return entityCountMultiplier.Evaluate(worldModel.observed.count) * Mathf.Max(0, value);
        }

    }
}