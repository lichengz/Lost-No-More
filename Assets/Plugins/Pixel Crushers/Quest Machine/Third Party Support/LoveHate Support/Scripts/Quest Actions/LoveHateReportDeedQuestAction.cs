// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using System;
using PixelCrushers.LoveHate;

namespace PixelCrushers.QuestMachine.LoveHateSupport
{

    /// <summary>
    /// This quest action reports a deed to Love/Hate.
    /// </summary>
    public class LoveHateReportDeedQuestAction : QuestAction
    {

        [Tooltip("Actor committing deed.")]
        [SerializeField]
        private StringField m_actorID;

        [Tooltip("Target deed is being done to.")]
        [SerializeField]
        private StringField m_targetID;

        [Tooltip("Use actor's Deed Reporter component.")]
        [SerializeField]
        private bool m_useActorDeedReporter = true;

        [SerializeField]
        private DeedTemplate m_deedTemplate = new DeedTemplate(string.Empty, string.Empty, 0, 0, new float[0], false, 10);

        public StringField actorID
        {
            get { return m_actorID; }
            set { m_actorID = value; }
        }

        public StringField targetID
        {
            get { return m_targetID; }
            set { m_targetID = value; }
        }

        public bool useActorDeedReporter
        {
            get { return m_useActorDeedReporter; }
            set { m_useActorDeedReporter = value; }
        }

        public DeedTemplate deedTemplate
        {
            get { return m_deedTemplate; }
            set { m_deedTemplate = value; }
        }

        public override void Execute()
        {
            base.Execute();
            var actor = LoveHateUtility.FindFactionMember(StringField.GetStringValue(actorID), quest.questerID);
            var target = LoveHateUtility.FindFactionMember(targetID, quest.questGiverID);
            if (actor == null)
            {
                if (Debug.isDebugBuild) Debug.LogWarning("Quest Machine: " + GetType().Name + " can't find actor '" + actorID + "' in the scene.");
            }
            else if (target == null)
            {
                if (Debug.isDebugBuild) Debug.LogWarning("Quest Machine: " + GetType().Name + " can't find target '" + targetID + "' in the scene.");
            }
            else if (useActorDeedReporter)
            {
                var deedReporter = actor.GetComponent<DeedReporter>();
                if (deedReporter == null)
                {
                    if (Debug.isDebugBuild) Debug.LogWarning("Quest Machine: " + GetType().Name + " can't find a Deed Reporter on '" + actorID + "' in the scene.", actor);
                }
                else
                {
                    deedReporter.ReportDeed(deedTemplate.tag, LoveHateUtility.FindFactionMember(targetID));
                }
            }
            else
            {
                var deed = Deed.GetNew(deedTemplate.tag, actor.factionID, target.factionID, deedTemplate.impact,
                                       deedTemplate.aggression, actor.GetPowerLevel(), deedTemplate.traits);
                LoveHateUtility.factionManager.CommitDeed(actor, deed, deedTemplate.requiresSight);
                Deed.Release(deed);
            }
        }

        public override string GetEditorName()
        {
            return "Deed: " + actorID + " " + deedTemplate.tag + " " + targetID;
        }

    }

}
