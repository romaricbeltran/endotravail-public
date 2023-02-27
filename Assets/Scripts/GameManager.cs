using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ScenarioCode
{
    Start,
    Scene_1_Popup_1,
    Scene_1_Dialogue_1,
    Scene_1_Action_1,
    Scene_1_Mission_1
}

public class GameManager : MonoBehaviour
{
    public static ScenarioCode CURRENT_SCENARIO_PROGRESS;
    public static int CURRENT_SCENE_PROGRESS;
    public static GameManager instance;

    public PopupManager popupManager;
    public DialogueManager dialogueManager;
    public ActionManager actionManager;
    public MissionManager missionManager;
    public TimelineManager timelineManager;
    public GameObject player;

    private PlayerInput playerInput;

    void Awake()
    {
        // Vérifie si une autre instance de GameManager existe déjà, dans ce cas détruit cet objet pour s'assurer qu'il n'y a qu'une seule instance
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // L'objet ne sera pas détruit lorsqu'une nouvelle scène sera chargée
        }
    }

    private void Start()
    {
        playerInput = player.GetComponent<PlayerInput>();
    }

    public void Start_Scenario()
    {
        CURRENT_SCENARIO_PROGRESS = ScenarioCode.Start;
    }

    public void Scene_1_Popup_1_Scenario()
    {
        CURRENT_SCENARIO_PROGRESS = ScenarioCode.Scene_1_Popup_1;
        Debug.Log("fnc + "+ CURRENT_SCENARIO_PROGRESS);
        popupManager.updatePopup(0);
    }

    public void Scene_1_Dialogue_1_Scenario()
    {
        CURRENT_SCENARIO_PROGRESS = ScenarioCode.Scene_1_Dialogue_1;
        dialogueManager.updateDialogue(0);

        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
    }

    public void Scene_1_Action_1_Scenario()
    {
        CURRENT_SCENARIO_PROGRESS = ScenarioCode.Scene_1_Action_1;
        timelineManager.SwitchTimelineClip();
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }
    }

    public void Scene_1_Mission_1_Scenario()
    {
        CURRENT_SCENARIO_PROGRESS = ScenarioCode.Scene_1_Mission_1;

        if (playerInput != null)
        {
            playerInput.enabled = true;
        }
    }
}
