// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// Custom inspector for LoveHateRelationshipUrgencyFunction.
    /// </summary>
    [CustomEditor(typeof(LoveHateRelationshipUrgencyFunction), true)]
    public class LoveHateRelationshipUrgencyFunctionEditor : Editor
    {

        private SerializedProperty descriptionProperty;
        private SerializedProperty relationshipIDProperty;
        private SerializedProperty multiplierProperty;
        private SerializedProperty entityCountMultiplierProperty;

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
            descriptionProperty = serializedObject.FindProperty("m_description");
            relationshipIDProperty = serializedObject.FindProperty("m_relationshipID");
            multiplierProperty = serializedObject.FindProperty("m_multiplier");
            entityCountMultiplierProperty = serializedObject.FindProperty("m_entityCountMultiplier");
            UnityEngine.Assertions.Assert.IsNotNull(descriptionProperty, "Quest Machine: Internal error - m_description is null.");
            UnityEngine.Assertions.Assert.IsNotNull(relationshipIDProperty, "Quest Machine: Internal error - m_relationshipID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(multiplierProperty, "Quest Machine: Internal error - m_multiplier is null.");
            UnityEngine.Assertions.Assert.IsNotNull(entityCountMultiplierProperty, "Quest Machine: Internal error - m_entityCountMultiplier is null.");
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
            if (descriptionProperty == null || relationshipIDProperty == null || multiplierProperty == null || entityCountMultiplierProperty == null) return;
            EditorGUILayout.PropertyField(descriptionProperty);
            relationshipIDProperty.intValue = LoveHateEditorUtility.DoLayoutRelationshipPopup("Relationship", relationshipIDProperty.intValue);
            EditorGUILayout.PropertyField(multiplierProperty);
            EditorGUILayout.PropertyField(entityCountMultiplierProperty);
        }

    }
}


