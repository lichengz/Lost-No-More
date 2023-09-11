// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// Custom inspector for LoveHateRelationshipQuestCondition assets.
    /// </summary>
    [CustomEditor(typeof(LoveHateRelationshipQuestCondition), true)]
    public class LoveHateRelationshipQuestConditionEditor : QuestSubassetEditor
    {

        private SerializedProperty judgeFactionIDProperty;
        private SerializedProperty subjectFactionIDProperty;
        private SerializedProperty relationshipIDProperty;
        private SerializedProperty modeProperty;
        private SerializedProperty valueProperty;
        private SerializedProperty frequencyToCheckProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
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
            modeProperty = serializedObject.FindProperty("m_mode");
            valueProperty = serializedObject.FindProperty("m_value");
            frequencyToCheckProperty = serializedObject.FindProperty("m_frequencyToCheck");
            UnityEngine.Assertions.Assert.IsNotNull(judgeFactionIDProperty, "Quest Machine: Internal error - m_judgeFactionID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(subjectFactionIDProperty, "Quest Machine: Internal error - m_subjectFactionID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(relationshipIDProperty, "Quest Machine: Internal error - m_relationshipID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(modeProperty, "Quest Machine: Internal error - m_mode is null.");
            UnityEngine.Assertions.Assert.IsNotNull(valueProperty, "Quest Machine: Internal error - m_value is null.");
            UnityEngine.Assertions.Assert.IsNotNull(frequencyToCheckProperty, "Quest Machine: Internal error - m_frequencyToCheck is null.");
        }

        protected override void Draw()
        {
            if (serializedObject == null) return;
            if (judgeFactionIDProperty == null || subjectFactionIDProperty == null || relationshipIDProperty == null || modeProperty == null || valueProperty == null) return;
            judgeFactionIDProperty.intValue = LoveHateEditorUtility.DoLayoutFactionPopup("Judge", judgeFactionIDProperty.intValue);
            subjectFactionIDProperty.intValue = LoveHateEditorUtility.DoLayoutFactionPopup("Subject", subjectFactionIDProperty.intValue);
            relationshipIDProperty.intValue = LoveHateEditorUtility.DoLayoutRelationshipPopup("Relationship", relationshipIDProperty.intValue);
            EditorGUILayout.PropertyField(modeProperty);
            EditorGUILayout.PropertyField(valueProperty);
            EditorGUILayout.PropertyField(frequencyToCheckProperty);
        }

    }
}
