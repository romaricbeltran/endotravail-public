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
    private int indexAction;

    private Queue<string> choices;
    private int indexChoices;

    public void updateDialogue(int indexAction) {
        foreach(string choice in actions[indexAction].GetChoices())
        {
            buttonsText[indexChoices].text = choice;
            indexChoices++;
        }
    }

    public void StartAction(ScenarioCode scenarioCode)
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
}
