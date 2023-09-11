// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// Returns an urgency score based on the observer's PAD. The observer's entity name or
    /// display name must match the name of a GameObject with a FactionMember component, or
    /// the ID of an entity in the scene.
    /// </summary>
    [CreateAssetMenu(menuName = "Pixel Crushers/Quest Machine/Generator/Urgency Functions/Love\u2215Hate/Urgency By PAD")]
    public class LoveHatePadUrgencyFunction : UrgencyFunction
    {

        public override string typeName { get { return "By PAD"; } }

        public override float Compute(WorldModel worldModel)
        {
            if (worldModel == null || worldModel.observer == null || worldModel.observer.entityType == null) return 0;
            var factionMember = LoveHateUtility.FindFactionMember(worldModel.observer.entityType.name, worldModel.observer.entityType.displayName);
            if (factionMember == null) return 0;
            return factionMember.pad.arousal + Mathf.Max(0, -factionMember.pad.pleasure);
        }

    }
}