using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Gère les dialogues pour une scène
public class DialogueManager : MonoBehaviour
{
    public GameManager gameManager;
    public MissionManager missionManager;
    public GameObject player;
    public Animator dialogueBoxAnimator;
    
    // UI
    public GameObject dialogueCanvas;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public float typingLetterInterval = 0.05f;

    // Liste des dialogues de la scène
    public List<Dialogue> dialogues;
    public Dictionary<int, Dialogue> dialogueDictionary;

    private Dialogue currentDialogue;

    // Liste des phrases d'un dialogue (nom + texte)
    private List<Sentence> sentences;

    private int indexDialogue;
    private int indexSentence;

    private void Awake()
    {
        dialogueDictionary = new Dictionary<int, Dialogue>();
        foreach (Dialogue dialogue in dialogues)
        {
            dialogueDictionary.Add(dialogue.code, dialogue);
        }
    }

    // On précharge le dialogue avec la première phrase
    public void LoadDialogue(int dialogueCode) {
        currentDialogue = FindDialogueByCode(dialogueCode);
        indexSentence = 0;
        sentences = currentDialogue.GetSentences();
        DisplayNextSentence();
    }

    public void StartDialogue() {
        dialogueBoxAnimator.SetBool("IsOpen", true);
    }

    public void EndDialogue() {
        dialogueBoxAnimator.SetBool("IsOpen", false);
        StopAllCoroutines();

        if (MissionManager.ON_MISSION_END)
        {
            missionManager.EndMission();
        }
        else
        {
            gameManager.updateProgression(currentDialogue.nextScenarioNodeCode);
        }
    }

    public bool HasNextSentence()
    {
        return indexSentence >= 0 && indexSentence < sentences.Count;
    }

    public void DisplayNextSentence()
    {
        if (HasNextSentence())
        {
            StopAllCoroutines();
            nameText.text = sentences[indexSentence].GetCharacter();
            StartCoroutine(TypeSentence(sentences[indexSentence].GetText()));
            indexSentence++;
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

    public Dialogue FindDialogueByCode(int dialogueCode) {
        if (dialogueDictionary.ContainsKey(dialogueCode))
        {
            return dialogueDictionary[dialogueCode];
        }
        else
        {
            Debug.LogError("Code de Dialogue invalide");
            return null;
        }
    }
}
