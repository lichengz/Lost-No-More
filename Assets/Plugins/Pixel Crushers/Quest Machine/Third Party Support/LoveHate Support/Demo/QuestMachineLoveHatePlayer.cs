// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.Example;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    [RequireComponent(typeof(RudimentaryPlayerController2D))]
    public class QuestMachineLoveHatePlayer : MonoBehaviour
    {

        private RudimentaryPlayerController2D controller;

        private void Awake()
        {
            controller = GetComponent<RudimentaryPlayerController2D>();
        }

        public void TalkToNPC()
        {
            if (controller.currentTarget == null) return;
            var questGiver = controller.currentTarget.GetComponent<QuestGiver>();
            if (questGiver != null)
            {
                questGiver.StartDialogue(gameObject);
            }
            else
            {
                QuestMachine.defaultQuestAlertUI.ShowAlert("Can't talk to " + controller.currentTarget.name + ". Not a quest giver.");
            }
        }

    }
}