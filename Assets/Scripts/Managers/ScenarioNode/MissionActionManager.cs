using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class TargetMissionList
{
	public List<TargetMission> targetMissions;
}

[System.Serializable]
public class TargetMission
{
	[SerializeField] public GameObject triggerObject;
	[SerializeField] public string nodeName;
}

public class MissionActionManager : BaseActionManager<MissionAction>
{
	public GameManager gameManager;
	public TimelineManager timelineManager;

	// UI
	public GameObject missionCanvas;
	public GameObject shadowBackground;
	public GameObject missionBox;
	public TextMeshProUGUI missionTitle;
	public TextMeshProUGUI missionText;
	public Button missionSkip;
	public Button acceptButton;
	public GameObject tutoAnalogicBox;
	public GameObject tutoAnalogicText;
	public GameObject tutoJoystickText;
	public Button tutoAnalogicButton;
	public GameObject tutoSkipBox;
	public Button tutoSkipButton;

	public bool isFirstMission;
	public List<TargetMissionList> targetMissionsLists;

	public List<TargetMission> currentTargetMissions;
	public HashSet<TargetMission> activatedTriggers = new HashSet<TargetMission>();
	private int targetMissionIndex = 0;

	public bool missionSkippable = false;

	public override void LoadData(MissionAction currentAction)
	{
		missionTitle.text = currentAction.Title;
		missionText.text = currentAction.Description;
		currentTargetMissions = targetMissionsLists[currentAction.ManagerTargetMissionListID].targetMissions;

		foreach ( TargetMission targetMission in currentTargetMissions )
		{
			TriggerObjectListener listener = targetMission.triggerObject.AddComponent<TriggerObjectListener>();
			listener.Initialize( this, targetMission );

			Debug.Log( $"Add Trigger Event for Target Mission: {targetMission.nodeName} on {targetMission.triggerObject.name}" );
		}
	}

	public override void StartAction()
	{
		missionCanvas.SetActive( true );

		acceptButton.onClick.AddListener( () => AcceptMission() );
	}

	public void AcceptMission()
	{
		acceptButton.gameObject.SetActive( false );

		if ( isFirstMission )
		{
			isFirstMission = false;
			StartTuto();
		}
		else
		{
			shadowBackground.SetActive( false );
			missionSkip.gameObject.SetActive( true );
			gameManager.SwitchPlayerInput( true );

			if ( Application.isMobilePlatform )
			{
				gameManager.joystickMobile.GetComponent<GraphicRaycaster>().enabled = true;
			}
			else
			{
				gameManager.analogicButtons.GetComponent<GraphicRaycaster>().enabled = true;
			}

			missionSkip.onClick.AddListener( () => SkipMission() );
		}
	}

	public void OnTriggerActivated(TargetMission targetMission)
	{
		missionSkippable = false;

		if ( !activatedTriggers.Contains( targetMission ) )
		{
			Destroy( targetMission.triggerObject.GetComponent<TriggerObjectListener>() );
			activatedTriggers.Add( targetMission );

			Debug.Log( $"Trigger activated: {targetMission.nodeName}" );

			// Indeed target missions are triggered by order
			targetMissionIndex++;

			if ( targetMissionIndex == currentTargetMissions.Count )
			{
				Debug.Log( "All triggers activated, ending mission." );
				EndAction();
			}

			ScenarioNode nextNode = timelineManager.FindScenarioNodeByName( targetMission.nodeName );

			if ( nextNode != null )
			{
				timelineManager.PlayScenarioNode( nextNode );
			}
			else
			{
				Debug.LogWarning( $"No scenario node found for TargetMission: {targetMission.nodeName}" );
			}
		}
	}

	public void SkipMission()
	{
		if ( missionSkippable )
		{
			if ( targetMissionIndex < currentTargetMissions.Count )
			{
				TargetMission skippedMission = currentTargetMissions[targetMissionIndex];

				Debug.Log( $"Target Mission skipped: {skippedMission.nodeName}" );

				OnTriggerActivated( skippedMission );
			}
		}
	}

	public override void EndAction()
	{
		foreach ( TargetMission targetMission in currentTargetMissions )
		{
			if ( !activatedTriggers.Contains( targetMission ) )
			{
				Destroy( targetMission.triggerObject.GetComponent<TriggerObjectListener>() );
			}
		}

		activatedTriggers.Clear();
		targetMissionIndex = 0;

		missionCanvas.SetActive( false );
		shadowBackground.SetActive( true );

		acceptButton.gameObject.SetActive( true );
		acceptButton.onClick.RemoveAllListeners();

		missionSkip.gameObject.SetActive( false );
		missionSkip.onClick.RemoveAllListeners();

		EventSystem.current.SetSelectedGameObject( null ); // Resets focus on the button
	}

	public void StartTuto()
	{
		tutoAnalogicBox.SetActive( true );
		if ( Application.isMobilePlatform )
		{
			tutoJoystickText.SetActive( true );
			gameManager.joystickMobile.SetActive( true );
		}
		else
		{
			tutoAnalogicText.SetActive( true );
			gameManager.analogicButtons.SetActive( true );
		}

		tutoAnalogicButton.onClick.AddListener( () => ContinueTuto() );
	}

	public void ContinueTuto()
	{
		tutoAnalogicBox.SetActive( false );
		tutoSkipBox.SetActive( true );
		missionSkip.gameObject.SetActive( true );

		tutoSkipButton.onClick.AddListener( () => EndTuto() );
	}

	public void EndTuto()
	{
		tutoSkipBox.SetActive( false );
		tutoAnalogicBox.SetActive( false );
		shadowBackground.SetActive( false );

		gameManager.analogicButtons.SetActive( false );
		gameManager.SwitchPlayerInput( true );

		missionSkip.onClick.AddListener( () => SkipMission() );
	}
}
