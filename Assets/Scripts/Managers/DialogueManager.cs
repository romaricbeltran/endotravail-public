using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Gère les dialogues pour une scène
public class DialogueManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    public Animator dialogueBoxAnimator;
    public bool isAnimated;
    
    // UI
    public GameObject dialogueCanvas;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public float typingLetterInterval = 0.05f;

    // Liste des dialogues de la scène
    public List<Dialogue> dialogues;
    private Dialogue currentDialogue;

    // Liste des phrases d'un dialogue (nom + texte)
    private List<Sentence> sentences;

    private int indexDialogue;
    private int indexSentence;

    // On précharge le dialogue avec la première phrase
    public void LoadDialogue(int dialogueCode) {
        idiotSearchDialogue(dialogueCode);
        indexSentence = 0;
        sentences = currentDialogue.GetSentences();
        DisplayNextSentence();
    }

    public void StartDialogue() {
        if (isAnimated) {
            dialogueBoxAnimator.SetBool("IsOpen", true);
        } else {
            dialogueCanvas.SetActive(true);
        }
    }

    public void EndDialogue() {
        if (isAnimated) {
            dialogueBoxAnimator.SetBool("IsOpen", false);
        } else {
            dialogueCanvas.SetActive(false);
        }

        StopAllCoroutines();

        Debug.Log("END Dialogue");
        gameManager.updateProgression(currentDialogue.nextNodeCode);
        // Cursor.lockState = CursorLockMode.None;
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

    // Les éléments doivent être dans l'ordre (plus performant que Find ou de faire un map)
    public void idiotSearchDialogue(int dialogueCode) {
        currentDialogue = dialogues[0];
    }
}
