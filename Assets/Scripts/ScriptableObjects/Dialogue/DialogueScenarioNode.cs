using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueScenarioNode", menuName = "ScriptableObjects/Dialogue/DialogueScenarioNode" )]
public class DialogueScenarioNode : BaseScenarioNode
{
	[SerializeField] private List<Sentence> sentences;

    public List<Sentence> Sentences { get => sentences; set => sentences = value; }
}
