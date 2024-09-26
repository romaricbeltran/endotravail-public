using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScenarioNodeManager : BaseScenarioNodeManager<EndGameScenarioNode>
{
	// UI
	public GameObject endGameCanvas;
	public TextMeshProUGUI mainText;
	public TextMeshProUGUI secondaryText;
	public Button Button;

	// CrossFade transition
	public Animator loadingScreenTransition;
	public float creditTime = 4.5f;
	public float transitionTime = 0.1f;
	public GameObject retardedUI;

	public override void LoadData(EndGameScenarioNode currentScenarioNode)
	{
		base.LoadData( currentScenarioNode );
		mainText.text = currentScenarioNode.MainText;
		secondaryText.text = currentScenarioNode.SecondaryText;
		StartNode();
	}

	public override void StartNode()
	{
		endGameCanvas.SetActive( true );
		loadingScreenTransition.SetBool( "Display", true );
		retardedUI.SetActive( true );
	}

	public override void EndNode()
	{
		base.EndNode();
	}
}
