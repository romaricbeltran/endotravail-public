using UnityEngine;
using System.Collections.Generic;

public abstract class BaseScenarioNodeManager<T> : MonoBehaviour, IScenarioNodeManager where T : BaseScenarioNode
{
	public event System.Action OnNodeCompleted;

	protected T currentNode;
	protected T nextNode;
	protected HashSet<string> temporaryFlags = new HashSet<string>();

	private bool nodeCompletedFlag;

	public void LoadData(BaseScenarioNode scenarioNode)
	{
		LoadData( scenarioNode as T ); // Run inherited virtual LoadData
	}

	public virtual void LoadData(T currentData)
	{
		currentNode = currentData;
		nextNode = null;
		nodeCompletedFlag = false;
		SaveScenarioNodeFlags();
	}

	public void SaveScenarioNodeFlags()
	{
		if ( currentNode.FlagList != null )
		{
			foreach ( Flag flag in currentNode.FlagList )
			{
				if ( flag.PersistedInProgress )
				{
					PlayerPrefs.SetInt( flag.FlagName, 1 ); // Sauvegarde 1 comme actif
					PlayerPrefs.Save();
				}
				else
				{
					temporaryFlags.Add( flag.FlagName );
				}
			}
		}
	}

	public abstract void StartNode();

	public virtual void EndNode()
	{
		if ( !nodeCompletedFlag )
		{
			nodeCompletedFlag = true;
			OnNodeCompleted?.Invoke();
		}
	}

	public BaseScenarioNode GetNextNode()
	{
		if ( currentNode.FlaggedScenarioNodeList != null )
		{
			foreach ( FlaggedScenarioNode flaggedNode in currentNode.FlaggedScenarioNodeList )
			{
				if ( IsFlagActive( flaggedNode.flag ) )
				{
					return flaggedNode.scenarioNode;
				}
			}
		}
		return nextNode;
	}

	public bool IsFlagActive(Flag flag)
	{
		if ( temporaryFlags.Contains( flag.FlagName ) )
		{
			return true;
		}

		return PlayerPrefs.GetInt( flag.FlagName, 0 ) == 1;
	}
}
