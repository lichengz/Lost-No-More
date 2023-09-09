using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueClip : PlayableAsset, ITimelineClipAsset
{
	[SerializeField] private DialogueBehaviour _template = default;

	[HideInInspector] public YarnSpinnerSO PlayDialogueEvent;
	
	// Having ClipCaps set to None makes sure that the clips can't be blended, extrapolated, looped, etc.
	public ClipCaps clipCaps
	{
		get { return ClipCaps.None; }
	}

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<DialogueBehaviour> playable = ScriptPlayable<DialogueBehaviour>.Create(graph, _template);
		
		_template.PlayDialogueEvent = PlayDialogueEvent;

		return playable;
	}
}
