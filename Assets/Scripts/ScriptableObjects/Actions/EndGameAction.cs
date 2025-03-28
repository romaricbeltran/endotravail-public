using UnityEngine;

[CreateAssetMenu(fileName = "EndGameAction", menuName = "ScriptableObjects/Actions/EndGameAction" )]
public class EndGameAction : BaseAction
{
	[SerializeField] [TextArea] private string mainText;
	[SerializeField] [TextArea] private string secondaryText;

	public string MainText { get => mainText; set => mainText = value; }
	public string SecondaryText { get => secondaryText; set => secondaryText = value; }
}
