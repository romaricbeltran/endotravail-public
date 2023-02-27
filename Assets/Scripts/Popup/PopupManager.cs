using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PopupManager : MonoBehaviour
{
    public GameManager gameManager;
    public TimelineManager timelineManager;
    public GameObject player;

    // UI
    public GameObject popupCanvas;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI sourceText;
    public Button nextButton;

    public List<Popup> popups;
    private int indexPopup;

    public void updatePopup(int indexPopup) {
        mainText.text = popups[indexPopup].GetMainText();
        sourceText.text = popups[indexPopup].GetSourceText();
    }

    public void StartPopup(ScenarioCode scenarioCode)
    {
        popupCanvas.SetActive(true);
    }

    public void EndPopup()
    {
        popupCanvas.SetActive(false);
        Debug.Log(GameManager.CURRENT_SCENARIO_PROGRESS);
        switch (GameManager.CURRENT_SCENARIO_PROGRESS)
        {
            case ScenarioCode.Scene_1_Popup_1:
                timelineManager.SwitchTimelineClip();
                break;
            default:
                break;
        }
        // player.GetComponent<PlayerInput>().actions.Enable();
        // //Cursor.lockState = CursorLockMode.None;
    }
}
