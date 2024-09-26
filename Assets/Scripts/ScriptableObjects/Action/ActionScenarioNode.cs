using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ActionScenarioNode", menuName = "ScriptableObjects/Action/ActionScenarioNode" )]
public class ActionScenarioNode : BaseScenarioNode
{
	[SerializeField] private List<ActionChoice> choices;

    public List<ActionChoice> Choices { get => choices; set => choices = value; }
}
