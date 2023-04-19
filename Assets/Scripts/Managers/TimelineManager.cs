using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using Unity.Services.Analytics;
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
    public BadgeManager badgeManager;
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
        // AnalyticsService.Instance.CustomData("gameProgress", new Dictionary<string, object>
        // {
        //     { "gameProgressStepName", GameManager.GAME_PROGRESSION }
        // });
        //Debug.Log("Playing Node :" + node.GetName());

        PlayableAsset clip = node.GetTimelineClip();
        EventType eventType = node.GetEventType();
        int eventIndex = node.GetEventIndex();
        
        if (clip)
        {
            director.Play(clip, node.GetDirectorWrapMode());
        }
        
        badgeManager.EndBadge();
        if (node.HasBadge())
        {
            badgeManager.LoadBadge(node.GetBadgeIndex());
        }

        LoadEvent(eventType, eventIndex);
    }

    public void LoadEvent(EventType eventType, int eventIndex) {
        // Node Event
        //Debug.Log("EventType + " + eventType.ToString());

        switch (eventType)
        {
            case EventType.Popup:
                //Debug.Log("Load Popup " + eventIndex);
                popupManager.LoadPopup(eventIndex);
                gameManager.SwitchPlayerInput(false);
                break;
            case EventType.Dialogue:
                //Debug.Log("Load Dialogue " + eventIndex);
                dialogueManager.LoadDialogue(eventIndex);
                gameManager.SwitchPlayerInput(false);
                break;
            case EventType.Mission:
                //Debug.Log("Load Mission " + eventIndex);
                missionManager.LoadMission(eventIndex);
                break;
            case EventType.Action:
                //Debug.Log("Load Action " + eventIndex);
                actionManager.LoadAction(eventIndex);
                gameManager.SwitchPlayerInput(false);
                break;
            case EventType.Camera:
                //Debug.Log("Load Camera");
                gameManager.SwitchPlayerInput(true);
                break;
            case EventType.End:
                //Debug.Log("End of chapter");
                levelLoader.LoadLevel(eventIndex);
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
