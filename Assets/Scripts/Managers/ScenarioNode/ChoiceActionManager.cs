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
	private List<Choice> validChoices;

	private const float canvasDelay = 1.25f;
	private const float buttonsDelay = 0f;

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
		validChoices = FilterValidChoices( new List<Choice>(currentAction.Choices) );

		if ( validChoices.Count > actionButtonsComponents.Count )
		{
			Debug.LogError( "Not enough action buttons for the number of choices." );
			return;
		}

		for ( int i = 0; i < validChoices.Count; i++ )
		{
			int index = i;
			TextMeshProUGUI buttonText = actionButtonsComponents[i].GetComponentInChildren<TextMeshProUGUI>();
			buttonText.text = validChoices[i].Description;
			actionButtonsComponents[i].onClick.AddListener( () => OnActionChoice( validChoices[index] ) );
		}
	}

	/*
	 * If the choice has valid HideFlag, not valid choice (skip).
	 * If the choice has valid EndChoiceFlag, only add the choice with the maximum flag points.
	 * Else, add valid choice.
	 *
	 * ForceShowEndChoiceFlag, add the Choice even if it has not the max points.
	 */
	private List<Choice> FilterValidChoices(List<Choice> choices)
	{
		List<Choice> filteredValidChoices = new List<Choice>();

		int maxPoints = 0;
		Choice validEndFlagChoice = null;

		foreach (Choice choice in choices)
		{
			if ( HasValidHideFlag( choice ) )
				continue;

			if (choice.EndChoiceFlags.Count > 0)
			{
				int maxEndChoiceFlagPoints = GetMaxEndChoiceFlagPoints(choice.EndChoiceFlags);

				if ( maxEndChoiceFlagPoints > maxPoints )
				{
					maxPoints = maxEndChoiceFlagPoints;
					validEndFlagChoice = choice;
				}

				if ( choice.ForceShowEndChoiceFlag )
				{
					filteredValidChoices.Add( choice );
				}
			}
			else
			{
				filteredValidChoices.Add( choice );
			}
		}

		if ( validEndFlagChoice != null )
		{
			filteredValidChoices.Add( validEndFlagChoice );
		}

		return filteredValidChoices;
	}

	private bool HasValidHideFlag(Choice choice)
	{
		if (choice.HideChoiceFlags.Count > 0)
		{
			foreach (Flag flag in choice.HideChoiceFlags)
			{
				if (flagManager.IsFlagValid(flag))
				{
					return true;
				}
			}
		}

		return false;
	}

	private int GetMaxEndChoiceFlagPoints(List<Flag> endChoiceFlags)
	{
		int maxPoints = 0;

		foreach ( Flag endChoiceFlag in endChoiceFlags )
		{
			if (flagManager.IsFlagValid(endChoiceFlag))
			{
				int flagPoints = flagManager.GetFlagPoints( endChoiceFlag );

				if ( flagPoints > maxPoints )
				{
					maxPoints = flagPoints;
				}
			}
		}

		return maxPoints;
	}

	public override void StartAction()
	{
		actionCanvas.SetActive( true );
		StartCoroutine( ActivateButtonsWithDelay() );
	}

	private IEnumerator ActivateButtonsWithDelay()
	{
		yield return new WaitForSeconds( canvasDelay );

		for ( int i = 0; i < validChoices.Count; i++ )
		{
			actionButtons[i].SetActive( true );
			yield return new WaitForSeconds( buttonsDelay );
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

		for ( int i = 0; i < validChoices.Count; i++ )
		{
			actionButtonsComponents[i].onClick.RemoveAllListeners();
			actionButtons[i].SetActive( false );
		}

		actionCanvas.SetActive( false );

		EventSystem.current.SetSelectedGameObject( null );  // Resets focus on the button
		base.EndAction();
	}
}
