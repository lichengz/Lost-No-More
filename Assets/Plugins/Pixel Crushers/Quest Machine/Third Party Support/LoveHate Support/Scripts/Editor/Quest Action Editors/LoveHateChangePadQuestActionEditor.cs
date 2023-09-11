// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// Custom inspector for LoveHateChangePadQuestAction assets.
    /// </summary>
    [CustomEditor(typeof(LoveHateChangePadQuestAction), true)]
    public class LoveHateChangePadQuestActionEditor : QuestSubassetEditor
    {

        protected override void Draw()
        {
            EditorGUILayout.HelpBox("Modifies the PAD values of a faction member. The Faction Member ID below must match the name of a GameObject with a FactionMember component or the Quest Machine ID of an entity in the scene.", MessageType.None);
            DrawDefaultInspector();
        }

    }
}
