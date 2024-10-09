using UnityEngine;

[CreateAssetMenu(fileName = "EndGameScenarioNode", menuName = "ScriptableObjects/End/EndGameScenarioNode" )]
public class EndGameScenarioNode : BaseScenarioNode
{
	[SerializeField] [TextArea] private string mainText;
	[SerializeField] [TextArea] private string secondaryText;

	public global::System.String MainText { get => mainText; set => mainText = value; }
	public global::System.String SecondaryText { get => secondaryText; set => secondaryText = value; }
}
