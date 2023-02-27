using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MissionManager : MonoBehaviour
{
    public GameManager gameManager;
    public TimelineManager timelineManager;
    public GameObject player;

    // UI
    public GameObject missionCanvas;
    public TextMeshProUGUI mainText;

    public List<Mission> missions;
    private int indexMission;

    public void updateMission(int indexMission) {
        mainText.text = missions[indexMission].GetMainText();
    }

    public void StartMission(ScenarioCode scenarioCode)
    {
        missionCanvas.SetActive(true);
    }

    public void EndMission()
    {
        missionCanvas.SetActive(false);
        // switch (GameManager.CURRENT_SCENARIO_PROGRESS)
        // {
        //     case ScenarioCode.Scene_1_Mission_1:
        //         timelineManager.SwitchTimelineClip();
        //         break;
        //     case ScenarioCode.Scene_1_Mission_2:
        //         indexMission = 1;
        //         break;
        //     case ScenarioCode.Scene_1_Mission_3:
        //         indexMission = 2;
        //         break;
        // }
        
        // player.GetComponent<PlayerInput>().actions.Enable();
        // //Cursor.lockState = CursorLockMode.None;
    }
}
