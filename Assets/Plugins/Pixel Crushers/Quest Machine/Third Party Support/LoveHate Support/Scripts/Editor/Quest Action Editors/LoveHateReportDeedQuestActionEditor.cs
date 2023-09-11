// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// Custom inspector for LoveHateReportDeedQuestAction assets.
    /// </summary>
    [CustomEditor(typeof(LoveHateReportDeedQuestAction), true)]
    public class LoveHateReportDeedQuestActionEditor : QuestSubassetEditor
    {

        private SerializedProperty actorIDProperty;
        private SerializedProperty targetIDProperty;
        private SerializedProperty useActorDeedReporterProperty;
        private SerializedProperty deedTemplateProperty;
        private SerializedProperty deedTagProperty;
        private SerializedProperty requiresSightProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            try
            {
                if (serializedObject == null) return;
            }
            catch (System.Exception)
            {
                return;
            }
            actorIDProperty = serializedObject.FindProperty("m_actorID");
            targetIDProperty = serializedObject.FindProperty("m_targetID");
            useActorDeedReporterProperty = serializedObject.FindProperty("m_useActorDeedReporter");
            deedTemplateProperty = serializedObject.FindProperty("m_deedTemplate");
            UnityEngine.Assertions.Assert.IsNotNull(actorIDProperty, "Quest Machine: Internal error - m_actorID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(targetIDProperty, "Quest Machine: Internal error - m_targetID is null.");
            UnityEngine.Assertions.Assert.IsNotNull(useActorDeedReporterProperty, "Quest Machine: Internal error - m_useActorDeedReporter is null.");
            UnityEngine.Assertions.Assert.IsNotNull(deedTemplateProperty, "Quest Machine: Internal error - m_deedTemplate is null.");
            deedTagProperty = deedTemplateProperty.FindPropertyRelative("tag");
            requiresSightProperty = deedTemplateProperty.FindPropertyRelative("requiresSight");
        }

        protected override void Draw()
        {
            if (serializedObject == null) return;
            if (actorIDProperty == null || targetIDProperty == null || useActorDeedReporterProperty == null || deedTemplateProperty == null || requiresSightProperty == null) return;
            EditorGUILayout.PropertyField(actorIDProperty);
            EditorGUILayout.PropertyField(targetIDProperty);
            EditorGUILayout.PropertyField(useActorDeedReporterProperty);
            EditorGUILayout.PropertyField(deedTagProperty);
            EditorGUILayout.PropertyField(requiresSightProperty);
            if (!useActorDeedReporterProperty.boolValue)
            {
                EditorGUILayout.PropertyField(deedTemplateProperty.FindPropertyRelative("radius"), new GUIContent("Radius (0=global)", "Radius at which deed is witnessable."), false);
                EditorGUILayout.PropertyField(deedTemplateProperty.FindPropertyRelative("impact"), new GUIContent("Impact", "Importance of deed."), false);
                EditorGUILayout.PropertyField(deedTemplateProperty.FindPropertyRelative("aggression"), new GUIContent("Aggression", "Aggressiveness of deed."), false);
                var db = LoveHateUtility.factionDatabase;
                if (db != null)
                {
                    var traitsProperty = deedTemplateProperty.FindPropertyRelative("traits");
                    traitsProperty.arraySize = db.personalityTraitDefinitions.Length;
                    for (int i = 0; i < db.personalityTraitDefinitions.Length; i++)
                    {
                        var traitName = db.personalityTraitDefinitions[i].name;
                        EditorGUILayout.Slider(traitsProperty.GetArrayElementAtIndex(i), -100, 100, new GUIContent(traitName, "Degree to which deed aligns to " + traitName));
                    }
                }
            }
        }

    }
}
