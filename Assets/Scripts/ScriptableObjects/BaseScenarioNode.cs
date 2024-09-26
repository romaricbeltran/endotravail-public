using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;

public class BaseScenarioNode : ScriptableObject
{
	[SerializeField] private string scenarioNodeName;
	[SerializeField] private PlayableAsset timelineClip;
	[SerializeField] private DirectorWrapMode directorWrapMode;
	[SerializeField] private Badge badge;
	[SerializeField] private List<Flag> flagList;
	[SerializeField] private List<FlaggedScenarioNode> flaggedScenarioNodeList;

	public string ScenarioNodeName { get => scenarioNodeName; set => scenarioNodeName = value; }
	public PlayableAsset TimelineClip { get => timelineClip; set => timelineClip = value; }
	public Badge Badge { get => badge; set => badge = value; }
	public DirectorWrapMode DirectorWrapMode { get => directorWrapMode; set => directorWrapMode = value; }
    public List<Flag> FlagList { get => flagList; set => flagList = value; }
    public List<FlaggedScenarioNode> FlaggedScenarioNodeList { get => flaggedScenarioNodeList; set => flaggedScenarioNodeList = value; }
}

[System.Serializable]
public class FlaggedScenarioNode
{
	public Flag flag;
	public BaseScenarioNode scenarioNode;
}
