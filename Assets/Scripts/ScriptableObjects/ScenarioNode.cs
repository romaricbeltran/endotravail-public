using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;

[System.Serializable]
public class ScenarioNode
{
	[SerializeField] private BaseAction action;
	[SerializeField] private Badge badge;
	[SerializeField] private List<Flag> flags;
	[SerializeField] private List<FlaggedScenarioNode> flaggedNodes;
	[SerializeField] private List<Branch> branches;

    public BaseAction Action { get => action; set => action = value; }
    public Badge Badge { get => badge; set => badge = value; }
    public List<Flag> Flags { get => flags; set => flags = value; }
    public List<FlaggedScenarioNode> FlaggedNodes { get => flaggedNodes; set => flaggedNodes = value; }
	public List<Branch> Branches { get => branches; set => branches = value; }
}

[System.Serializable]
public class FlaggedScenarioNode
{
    [SerializeField] private Flag flag;
	[SerializeField] private string nodeName;

    public Flag Flag { get => flag; set => flag = value; }
    public global::System.String NodeName { get => nodeName; set => nodeName = value; }
}

[System.Serializable]
public class Branch
{
	[SerializeField] private List<ScenarioNode> nodes;

	public List<ScenarioNode> Nodes { get => nodes; set => nodes = value; }
}
