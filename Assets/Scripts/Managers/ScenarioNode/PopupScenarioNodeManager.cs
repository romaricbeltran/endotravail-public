using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupScenarioNodeManager : BaseScenarioNodeManager<PopupScenarioNode>
{
	// UI
	public GameObject popupCanvas;
	public TextMeshProUGUI mainText;
	public TextMeshProUGUI sourceText;
	public Button nextButton;

	public override void LoadData(PopupScenarioNode currentScenarioNode)
	{
		base.LoadData( currentScenarioNode );
		mainText.text = currentScenarioNode.MainText;
		sourceText.text = currentScenarioNode.SourceText;
	}

	public override void StartNode()
	{
		popupCanvas.SetActive( true );
	}

	public override void EndNode()
	{
		popupCanvas.SetActive( false );
		base.EndNode();
	}
}
