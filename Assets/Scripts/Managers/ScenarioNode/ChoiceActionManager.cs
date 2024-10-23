using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class ChoiceActionManager : BaseActionManager<ChoiceAction>
{
	public FlagManager flagManager;

	// UI
	public GameObject actionCanvas;
	public List<GameObject> actionButtons;

	private List<Button> actionButtonsComponents;
	private float canvasDelay = 1.75f;
	private float buttonsDelay = 0.75f;

	public override void LoadData(ChoiceAction currentAction)
	{
		actionButtonsComponents = new();

		for ( int i = 0; i < currentAction.Choices.Count; i++ )
		{
			actionButtonsComponents.Add(actionButtons[i].GetComponent<Button>());
			actionButtonsComponents[i].onClick.RemoveAllListeners();

			TextMeshProUGUI buttonText = actionButtonsComponents[i].GetComponentInChildren<TextMeshProUGUI>();
			buttonText.text = currentAction.Choices[i].Description;
		}
	}

	public override void StartAction()
	{
		actionCanvas.SetActive( true );
		StartCoroutine( ActiveButtonsWithDelay() );
	}

	private IEnumerator ActiveButtonsWithDelay()
	{
		yield return new WaitForSeconds( canvasDelay ); 

		for ( int i = 0; i < currentAction.Choices.Count; i++ )
		{
			int index = i;
			actionButtonsComponents[i].onClick.AddListener( () => OnActionChoice( currentAction.Choices[index] ) );
			actionButtons[i].SetActive( true );
			yield return new WaitForSeconds( buttonsDelay );
		}
	}

	private void OnActionChoice(Choice choice)
	{
		if ( choice.Flags != null )
		{
			flagManager.SaveFlags( choice.Flags );
		}

		nextScenarioNodeName = choice.NodeName;
		EndAction();
	}

	public override void EndAction()
	{
		foreach ( GameObject actionButton in actionButtons )
		{
			actionButton.SetActive( false );
		}

		EventSystem.current.SetSelectedGameObject( null ); // Resets focus on the button

		actionCanvas.SetActive( false );
		base.EndAction();
	}
}
