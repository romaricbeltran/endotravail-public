using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public Button nextButton;
    public GameObject sideViewCamera;
    public GameObject player;

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        nextButton.onClick.AddListener(DisplayNextSentence);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Démarrer un dialogue avec une liste de phrases
    public void StartDialogue(string[] lines)
    {
        sentences = new Queue<string>();

        foreach (string sentence in lines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

        // Afficher la Dialog Box & les phrases
        dialogueBox.SetActive(true);

        // Switcher la caméra
        sideViewCamera.SetActive(true);
    }

    // Afficher la phrase suivante du dialogue
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    // Arrêter le dialogue
    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        sideViewCamera.SetActive(false);
        player.GetComponent<PlayerInput>().actions.Enable();
        Cursor.lockState = CursorLockMode.None;
    }
}
