using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    public Animator dialogueBoxAnimator;
    
    // UI
    public GameObject dialogueCanvas;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public float typingLetterInterval = 0.05f;

    public List<Dialogue> dialogues;
    private int indexDialogue;

    // Monologues
    private List<Monologue> listMonologues;
    private string currentCharacter;
    private List<string> currentSentences;

    private int indexMonologue;
    private int indexSentence;

    public void updateDialogue(int indexDialogue) {
        listMonologues = dialogues[indexDialogue].GetMonologues();

        currentCharacter = listMonologues[indexMonologue].GetCharacter();
        currentSentences = listMonologues[indexMonologue].GetSentences();

        GetNextSentence();

        dialogueBoxAnimator.SetBool("IsOpen", true);
    }

    public void clear() {
        listMonologues.Clear();
        currentCharacter = null;
        currentSentences.Clear();
        indexMonologue = 0;
        indexSentence = 0;
    }

    public void StartDialogue(ScenarioCode scenarioCode)
    {
        // dialogueCanvas.SetActive(true);
        dialogueBoxAnimator.SetBool("IsOpen", true);
    }

    public bool HasNextMonologue()
    {
        return indexMonologue < (listMonologues.Count - 1);
    }

    public bool HasNextSentence()
    {
        return indexSentence < currentSentences.Count;
    }

    public void GetNextSentence()
    {
        if (HasNextSentence())
        {
            string currentSentence = currentSentences[indexSentence];
            indexSentence++;
            nameText.text = currentCharacter;
            dialogueText.text = currentSentence;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentSentence));
        } else if (HasNextMonologue())
        {
            indexMonologue++;
            currentCharacter = listMonologues[indexMonologue].GetCharacter();
            currentSentences = listMonologues[indexMonologue].GetSentences();
            indexSentence = 0;
            GetNextSentence();
        } else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingLetterInterval);
        }
    }

    public void EndDialogue()
    {
        Debug.Log("END");
        dialogueBoxAnimator.SetBool("IsOpen", false);
        // dialogueCanvas.SetActive(false);
        StopAllCoroutines();
        clear();

        switch (GameManager.CURRENT_SCENARIO_PROGRESS)
        {
            case ScenarioCode.Scene_1_Dialogue_1:
                gameManager.Scene_1_Action_1_Scenario();
                break;
            default:
                break;
        }
        // Cursor.lockState = CursorLockMode.None;
    }
}
