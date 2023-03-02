using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public GameObject player;

    // UI
    public GameObject actionBox;
    public List<Button> buttons;
    public List<TextMeshProUGUI> buttonsText;

    public List<Action> actions;
    public Dictionary<int, Action> actionDictionary;

    private Action currentAction;

    private Queue<string> choices;
    private int indexChoices;

    private void Awake()
    {
        actionDictionary = new Dictionary<int, Action>();
        foreach (Action action in actions)
        {
            actionDictionary.Add(action.code, action);
        }
    }

    // public void updateAction(int indexAction) {
    //     foreach(string choice in actions[indexAction].GetChoices())
    //     {
    //         buttonsText[indexChoices].text = choice;
    //         indexChoices++;
    //     }
    // }

    public void StartAction()
    {
        actionBox.SetActive(true);
    }

    public void EndAction()
    {
        actionBox.SetActive(false);
        indexChoices = 0;
        // GameObject.Find("Player").GetComponent<PlayerInput>().actions.Enable();
        // Cursor.lockState = CursorLockMode.None;
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
