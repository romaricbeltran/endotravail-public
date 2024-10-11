using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameActionManager : BaseActionManager<EndGameAction>
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

	public override void LoadData(EndGameAction currentNode)
	{
		mainText.text = currentAction.MainText;
		secondaryText.text = currentAction.SecondaryText;
		StartAction();
	}

	public override void StartAction()
	{
		endGameCanvas.SetActive( true );
		loadingScreenTransition.SetBool( "Display", true );
		retardedUI.SetActive( true );
		GameManager.ResetProgress();
	}
}
