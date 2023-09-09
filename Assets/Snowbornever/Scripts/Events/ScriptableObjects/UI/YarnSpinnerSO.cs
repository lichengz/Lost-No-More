using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using Yarn.Unity;

[CreateAssetMenu(menuName = "Events/UI/Yarn Spinner Dialogue")]
public class YarnSpinnerSO : DescriptionBaseSO
{
	public YarnProject yarnProject;
	public string startTitle = "Start";

	// public UnityAction<LocalizedString, ActorSO> OnEventRaised;
	//
	// public void RaiseEvent(LocalizedString line, ActorSO actor)
	// {
	// 	if (OnEventRaised != null)
	// 		OnEventRaised.Invoke(line, actor);
	// }
}
