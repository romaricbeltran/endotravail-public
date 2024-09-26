using UnityEngine;

[CreateAssetMenu(fileName = "PopupScenarioNode", menuName = "ScriptableObjects/Popup/PopupScenarioNode" )]
public class PopupScenarioNode : BaseScenarioNode
{
	[SerializeField] [TextArea] private string mainText;
	[SerializeField] [TextArea] private string sourceText;

	public global::System.String MainText { get => mainText; set => mainText = value; }
	public global::System.String SourceText { get => sourceText; set => sourceText = value; }
}
