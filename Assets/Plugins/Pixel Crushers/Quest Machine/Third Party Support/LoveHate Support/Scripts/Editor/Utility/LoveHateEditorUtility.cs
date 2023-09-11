// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;
using PixelCrushers.LoveHate;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    public static class LoveHateEditorUtility
    {

        public static int DoLayoutFactionPopup(string label, int index)
        {
            if (LoveHateUtility.factionNames != null)
            {
                return EditorGUILayout.Popup(label, index, LoveHateUtility.factionNames);
            }
            else
            {
                return EditorGUILayout.IntField(label, index);
            }
        }

        public static int DoLayoutRelationshipPopup(string label, int index)
        {
            if (LoveHateUtility.relationshipNames != null)
            {
                return EditorGUILayout.Popup(label, index, LoveHateUtility.relationshipNames);
            }
            else
            {
                return EditorGUILayout.IntField(label, index);
            }
        }

    }

}
