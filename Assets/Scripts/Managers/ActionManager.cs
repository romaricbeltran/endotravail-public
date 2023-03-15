using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public GameManager gameManager;
    public MissionManager missionManager;
    public GameObject player;

    // UI
    public GameObject actionBox;
    public List<GameObject> actionButtons;

    // Liste des actions de la scène
    public List<Action> actions;
    public Dictionary<int, Action> actionDictionary;

    private Action currentAction;

    // Liste des choix d'une action
    private List<string> choices;
    private List<int> scenarioNodeNextCodes;

    private void Awake()
    {
        actionDictionary = new Dictionary<int, Action>();
        foreach (Action action in actions)
        {
            actionDictionary.Add(action.code, action);
        }
    }

    public void LoadAction(int actionCode)
    {
        currentAction = FindActionByCode(actionCode);
        choices = currentAction.GetChoices();
        scenarioNodeNextCodes = currentAction.GetScenarioNodeNextCodes();
        for (int i = 0; i < choices.Count; i++)
        {
            int indexChoices = i;

            actionButtons[indexChoices].SetActive(true);

            Button buttonComponent = actionButtons[indexChoices].GetComponent<Button>();
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(() => OnActionChoice(indexChoices));

            TextMeshProUGUI buttonText = actionButtons[indexChoices].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = choices[indexChoices];
        }
    }

    private void OnActionChoice(int choiceIndex)
    {
        Debug.Log("Action choisie : " + choices[choiceIndex]);
        currentAction.SetChoosenActionIndex(scenarioNodeNextCodes[choiceIndex]);
        EndAction();

        // Désactive le focus sur le bouton qui a été cliqué
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void StartAction()
    {
        actionBox.SetActive(true);
    }

    public void EndAction()
    {
        for (int i = 0; i < actionButtons.Count; i++)
        {
            actionButtons[i].SetActive(false);
        }
        
        actionBox.SetActive(false);

        if (MissionManager.ON_MISSION_END)
        {
            MissionManager.WAS_ACTION_MISSION_COMPONENT = true;
            missionManager.EndMission();
        }
        
        gameManager.updateProgression(currentAction.GetChoosenActionIndex());
    }

    public Action FindActionByCode(int actionCode) {
        if (actionDictionary.ContainsKey(actionCode))
        {
            return actionDictionary[actionCode];
        }
        else
        {
            Debug.LogError("Code d'Action invalide");
            return null;
        }
    }
}
