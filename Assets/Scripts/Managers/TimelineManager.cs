using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public enum EventType {
    popup,
    dialogue,
    mission,
    action
}

public class TimelineManager : MonoBehaviour
{
    public PlayableAsset[] timelineClips;

// New
    public GameManager gameManager;
    public PopupManager popupManager;
    public DialogueManager dialogueManager;
    public ActionManager actionManager;
    public MissionManager missionManager;
    public GameObject player;

    public List<ScenarioNode> scenario;

    private PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void PlayScenario(int scenarioNodeCode)
    {
        // Scenario Node
        ScenarioNode scenarioNode = idiotSearchScenarioNode(scenarioNodeCode);
        NodeEvent nodeEvent = scenarioNode.GetNodeEvent();
        PlayableAsset currentClip = scenarioNode.GetTimelineClip();

        if (currentClip)
        {
            director.playableAsset = currentClip;
            director.Play();
        }

        // Node Event
        Debug.Log("EventType + " + nodeEvent.type.ToString());

        switch (nodeEvent.type)
        {
            case EventType.popup:
                popupManager.LoadPopup(nodeEvent.code);
                gameManager.SwitchPlayerInput(true);
                break;
            case EventType.dialogue:
                dialogueManager.LoadDialogue(nodeEvent.code);
                gameManager.SwitchPlayerInput(true);
                break;
            case EventType.mission:
                missionManager.LoadMission(nodeEvent.code);
                gameManager.SwitchPlayerInput(true);
                break;
            default:
                break;
        }
    }

    public ScenarioNode idiotSearchScenarioNode(int nodeCode) {
        return scenario[nodeCode];
    }
}
