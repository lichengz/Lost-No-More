// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using PixelCrushers.LoveHate;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    public delegate void RumorParameterDelegate(Rumor rumor);

    [AddComponentMenu("")] // Added manually by scripts.
    public class RememberDeedNotifier : MonoBehaviour, IRememberDeedEventHandler
    {

        public event RumorParameterDelegate rememberedDeed = delegate { };

        public void OnRememberDeed(Rumor rumor)
        {
            rememberedDeed(rumor);
        }

    }

}
