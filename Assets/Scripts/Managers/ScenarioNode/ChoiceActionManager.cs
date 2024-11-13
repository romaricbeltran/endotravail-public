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
	private const float canvasDelay = 1.75f;
	private const float buttonsDelay = 0.75f;

	private void Awake()
	{
		actionButtonsComponents = new List<Button>();

		foreach ( GameObject actionButton in actionButtons )
		{
			var buttonComponent = actionButton.GetComponent<Button>();
			if ( buttonComponent != null )
			{
				actionButtonsComponents.Add( buttonComponent );
			}
			else
			{
				Debug.LogWarning( $"Button component not found in {actionButton.name}" );
			}
		}
	}

	public override void LoadData(ChoiceAction currentAction)
	{
		if ( currentAction.Choices.Count > actionButtonsComponents.Count )
		{
			Debug.LogError( "Not enough action buttons for the number of choices." );
			return;
		}

		for ( int i = 0; i < currentAction.Choices.Count; i++ )
		{
			int index = i;
			TextMeshProUGUI buttonText = actionButtonsComponents[i].GetComponentInChildren<TextMeshProUGUI>();
			buttonText.text = currentAction.Choices[i].Description;
			actionButtonsComponents[i].onClick.AddListener( () => OnActionChoice( currentAction.Choices[index] ) );
		}
	}

	public override void StartAction()
	{
		actionCanvas.SetActive( true );
		StartCoroutine( ActivateButtonsWithDelay() );
	}

	private IEnumerator ActivateButtonsWithDelay()
	{
		yield return new WaitForSeconds( canvasDelay );

		for ( int i = 0; i < currentAction.Choices.Count; i++ )
		{
			bool shouldActivateButton = true;

			if ( currentAction.Choices[i].HideChoiceFlags.Count > 0 )
			{
				foreach ( Flag flag in currentAction.Choices[i].HideChoiceFlags )
				{
					if ( flagManager.IsFlagValid( flag ) )
					{
						shouldActivateButton = false;
						break;
					}
				}
			}

			if ( currentAction.Choices[i].ShowChoiceFlags.Count > 0 )
			{
				shouldActivateButton = false;

				foreach ( Flag flag in currentAction.Choices[i].ShowChoiceFlags )
				{
					if ( flagManager.IsFlagValid( flag ) )
					{
						shouldActivateButton = true;
						break;
					}
				}
			}

			if ( shouldActivateButton )
			{
				actionButtons[i].SetActive( true );
				yield return new WaitForSeconds( buttonsDelay );
			}
		}
	}

	private void OnActionChoice(Choice choice)
	{
		if ( choice.Flags.Count > 0 )
		{
			flagManager.SaveFlags( choice.Flags );
		}

		if (choice.ActivateBackToMissionPOV)
		{
			activateBackToMissionPOV = true;
		}

		nextScenarioNodeName = choice.NodeName;
		EndAction();
	}

	public override void EndAction()
	{
		StopAllCoroutines();

		for ( int i = 0; i < currentAction.Choices.Count; i++ )
		{
			actionButtonsComponents[i].onClick.RemoveAllListeners();
			actionButtons[i].SetActive( false );
		}

		actionCanvas.SetActive( false );

		EventSystem.current.SetSelectedGameObject( null );  // Resets focus on the button
		base.EndAction();
	}
}
