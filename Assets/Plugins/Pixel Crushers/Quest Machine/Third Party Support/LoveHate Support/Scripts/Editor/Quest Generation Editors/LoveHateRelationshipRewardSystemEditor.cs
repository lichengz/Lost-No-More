// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// Custom inspector for LoveHateRelationshipRewardSystem.
    /// </summary>
    [CustomEditor(typeof(LoveHateRelationshipRewardSystem), true)]
    public class LoveHateRelationshipRewardSystemEditor : Editor
    {

        private SerializedProperty judgeFactionIDProperty;
        private SerializedProperty subjectFactionIDProperty;
        private SerializedProperty relationshipIDProperty;
        private SerializedProperty multiplierProperty;
        private SerializedProperty rewardTextProperty;

        protected virtual void OnEnable()
        {
            try
            {
                if (serializedObject == null) return;
            }
            catch (System.NullReferenceException)
            {
                return;
            }
            judgeFactionIDProperty = serializedObject.FindProperty("m_judgeFactionID");
            subjectFactionIDProperty = serializedObject.FindProperty("m_subjectFactionID");
            relationshipIDProperty = serializedObject.FindProperty("m_relationshipID");
            multiplierProperty = serializedObject.FindProperty("m_multiplier");
            rewardTextProperty = serializedObject.FindProperty("m_rewardText");
            UnityEngine.Assertions.Assert.IsNotNull(judgeFactionIDProperty, "Quest Machine: Internal error - m_judgeFactionID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(subjectFactionIDProperty, "Quest Machine: Internal error - m_subjectFactionID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(relationshipIDProperty, "Quest Machine: Internal error - m_relationshipID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(multiplierProperty, "Quest Machine: Internal error - m_multiplier is null.");
            UnityEngine.Assertions.Assert.IsNotNull(rewardTextProperty, "Quest Machine: Internal error - m_rewardText is null.");
        }

        public override void OnInspectorGUI()
        {
            if (serializedObject == null) return;
            serializedObject.Update();
            Draw();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void Draw()
        {
            if (judgeFactionIDProperty == null || subjectFactionIDProperty == null || relationshipIDProperty == null || multiplierProperty == null || rewardTextProperty == null) return;
            EditorGUILayout.HelpBox("This reward system increases (or decreases) the value of a Love/Hate faction's relationship.", MessageType.None);
            judgeFactionIDProperty.intValue = LoveHateEditorUtility.DoLayoutFactionPopup("Judge", judgeFactionIDProperty.intValue);
            subjectFactionIDProperty.intValue = LoveHateEditorUtility.DoLayoutFactionPopup("Subject", subjectFactionIDProperty.intValue);
            relationshipIDProperty.intValue = LoveHateEditorUtility.DoLayoutRelationshipPopup("Relationship", relationshipIDProperty.intValue);
            EditorGUILayout.PropertyField(multiplierProperty);
            EditorGUILayout.HelpBox("In the reward text, {0} is the amount to change the relationship, {1} is the relationship type, and {2} is the faction whose relationship to update.", MessageType.None);
            EditorGUILayout.PropertyField(rewardTextProperty);
        }

    }
}


