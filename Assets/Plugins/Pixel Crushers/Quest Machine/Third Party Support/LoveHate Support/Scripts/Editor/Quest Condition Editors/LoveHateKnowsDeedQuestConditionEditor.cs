// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// Custom inspector for LoveHateKnowsDeedQuestCondition assets.
    /// </summary>
    [CustomEditor(typeof(LoveHateKnowsDeedQuestCondition), true)]
    public class LoveHateKnowsDeedQuestConditionEditor : QuestSubassetEditor
    {

        private SerializedProperty factionMemberIDProperty;
        private SerializedProperty actorFactionIDProperty;
        private SerializedProperty targetFactionIDProperty;
        private SerializedProperty deedTagProperty;
        private SerializedProperty acceptAlreadyKnownProperty;

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
            factionMemberIDProperty = serializedObject.FindProperty("m_factionMemberID");
            actorFactionIDProperty = serializedObject.FindProperty("m_actorID");
            targetFactionIDProperty = serializedObject.FindProperty("m_targetID");
            deedTagProperty = serializedObject.FindProperty("m_deedTag");
            acceptAlreadyKnownProperty = serializedObject.FindProperty("m_acceptAlreadyKnown");
            UnityEngine.Assertions.Assert.IsNotNull(factionMemberIDProperty, "Quest Machine: Internal error - m_factionMemberID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(actorFactionIDProperty, "Quest Machine: Internal error - m_actorID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(targetFactionIDProperty, "Quest Machine: Internal error - m_targetID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(deedTagProperty, "Quest Machine: Internal error - m_deedTag is null.");
            UnityEngine.Assertions.Assert.IsNotNull(acceptAlreadyKnownProperty, "Quest Machine: Internal error - m_acceptAlreadyKnown is null.");
        }

        protected override void Draw()
        {
            if (serializedObject == null) return;
            if (factionMemberIDProperty == null || actorFactionIDProperty == null || targetFactionIDProperty == null || deedTagProperty == null || acceptAlreadyKnownProperty == null) return;
            EditorGUILayout.PropertyField(factionMemberIDProperty);
            actorFactionIDProperty.intValue = LoveHateEditorUtility.DoLayoutFactionPopup("Actor", actorFactionIDProperty.intValue);
            targetFactionIDProperty.intValue = LoveHateEditorUtility.DoLayoutFactionPopup("Target", targetFactionIDProperty.intValue);
            EditorGUILayout.PropertyField(deedTagProperty);
            EditorGUILayout.PropertyField(acceptAlreadyKnownProperty);
        }

    }
}
