using UnityEngine;

[CreateAssetMenu(fileName = "EndGameAction", menuName = "ScriptableObjects/Actions/EndGameAction" )]
public class EndGameAction : BaseAction
{
	[SerializeField] [TextArea] private string mainText;
	[SerializeField] [TextArea] private string secondaryText;

	public global::System.String MainText { get => mainText; set => mainText = value; }
	public global::System.String SecondaryText { get => secondaryText; set => secondaryText = value; }
}
