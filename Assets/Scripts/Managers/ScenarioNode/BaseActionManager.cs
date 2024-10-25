using UnityEngine;
using System.Collections.Generic;

public abstract class BaseActionManager<T> : MonoBehaviour, IActionManager where T : BaseAction
{
	public event System.Action OnNodeCompleted;
	public string nextScenarioNodeName { get; set; }
	public bool activateBackToMissionPOV { get; set; }

	protected T currentAction;

	private bool loadingNextNode;

	public void LoadData(BaseAction action)
	{
		currentAction = action as T;
		nextScenarioNodeName = null;
		activateBackToMissionPOV = false;
		loadingNextNode = false;

		LoadData( currentAction ); // Run inherited LoadData
	}

	public abstract void LoadData(T currentData);

	public abstract void StartAction();

	public virtual void EndAction()
	{
		if ( !loadingNextNode )
		{
			loadingNextNode = true;
			OnNodeCompleted?.Invoke();
		}
	}
}
