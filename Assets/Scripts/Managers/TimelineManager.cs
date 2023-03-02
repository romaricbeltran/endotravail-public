using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public LevelLoader levelLoader;
    public GameManager gameManager;
    public PopupManager popupManager;
    public DialogueManager dialogueManager;
    public ActionManager actionManager;
    public MissionManager missionManager;
    public GameObject player;

    public List<ScenarioNode> scenario;
    public Dictionary<int, ScenarioNode> scenarioNodeDictionary;

    private PlayableDirector director;

    private void Awake()
    {
        scenarioNodeDictionary = new Dictionary<int, ScenarioNode>();
        foreach (ScenarioNode node in scenario)
        {
            scenarioNodeDictionary.Add(node.scenarioNodeCode, node);
        }

        director = GetComponent<PlayableDirector>();
    }

    public void PlayScenario(int scenarioNodeCode)
    {
        ScenarioNode node = SearchScenarioNodeByCode(scenarioNodeCode);
        GameManager.GAME_PROGRESSION = node.GetName();
        Debug.Log("Playing Node :" + node.GetName());

        PlayableAsset clip = node.GetTimelineClip();
        EventType eventType = node.GetEventType();
        int eventIndex = node.GetEventIndex();
        
        if (clip)
        {
            director.playableAsset = clip;
            director.Play();
        }

        // Node Event
        Debug.Log("EventType + " + eventType.ToString());

        switch (eventType)
        {
            case EventType.Popup:
                Debug.Log("Load Popup " + eventIndex);
                popupManager.LoadPopup(eventIndex);
                gameManager.SwitchPlayerInput(false);
                break;
            case EventType.Dialogue:
                Debug.Log("Load Dialogue " + eventIndex);
                dialogueManager.LoadDialogue(eventIndex);
                gameManager.SwitchPlayerInput(false);
                break;
            case EventType.Mission:
                Debug.Log("Load Mission " + eventIndex);
                missionManager.LoadMission(eventIndex);
                gameManager.SwitchPlayerInput(true);
                break;
            case EventType.End:
                Debug.Log("End of chapter");
                levelLoader.LoadLevel(2);
                break;
            default:
                break;
        }
    }

    public ScenarioNode SearchScenarioNodeByCode(int nodeCode) {
        if (scenarioNodeDictionary.ContainsKey(nodeCode))
        {
            return scenarioNodeDictionary[nodeCode];
        }
        else
        {
            Debug.LogError("Code de ScenarioNode invalide");
            return null;
        }
    }
}
