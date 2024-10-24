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
	public Button tutoAnalogicButton;
	public GameObject tutoSkipBox;
	public Button tutoSkipButton;

	public bool isFirstMission;
	public List<TargetMissionList> targetMissionsLists;

	private List<TargetMission> currentTargetMissions;
	private HashSet<TargetMission> activatedTriggers = new HashSet<TargetMission>();

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

	public void OnTriggerActivated(TargetMission targetMission)
	{
		if ( !activatedTriggers.Contains( targetMission ) )
		{
			Destroy( targetMission.triggerObject.GetComponent<TriggerObjectListener>() );
			activatedTriggers.Add( targetMission );
			Debug.Log( $"Trigger activated: {targetMission.nodeName}" );

			ScenarioNode nextNode = timelineManager.FindScenarioNodeByName( targetMission.nodeName );

			if ( nextNode != null )
			{
				timelineManager.PlayScenarioNode( nextNode );
			}
			else
			{
				Debug.LogWarning( $"No scenario node found for TargetMission: {targetMission.nodeName}" );
			}

			if ( activatedTriggers.Count == currentTargetMissions.Count )
			{
                Debug.Log("All triggers activated, ending mission.");
                EndAction();
			}
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
			gameManager.SwitchPlayerInput( true );
			gameManager.analogicButtons.GetComponent<GraphicRaycaster>().enabled = true;

			missionSkip.onClick.AddListener( () => SkipMission() );
		}
	}

	public void SkipMission()
	{
		if ( currentTargetMissions.Count > 0 )
		{
			TargetMission skippedMission = currentTargetMissions[0];
			currentTargetMissions.RemoveAt( 0 ); // Skip target mission in order

			Debug.Log( $"Mission skipped: {skippedMission.nodeName}" );

			OnTriggerActivated( skippedMission );

			if ( currentTargetMissions.Count == 0 )
			{
				EndAction();
			}
		}
	}

	public override void EndAction()
	{
		activatedTriggers.Clear();
		missionCanvas.SetActive( false );
		EventSystem.current.SetSelectedGameObject( null ); // Resets focus on the button
	}

	public void StartTuto()
	{
		tutoAnalogicBox.SetActive( true );
		gameManager.analogicButtons.SetActive( true );

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

		gameManager.SwitchPlayerInput( true );

		missionSkip.onClick.AddListener( () => SkipMission() );
	}
}
