using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDialogue : MonoBehaviour
{
    public string[] dialogueLines;

    private DialogueManager dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.StartDialogue(dialogueLines);

            // Tourner le PNJ vers le player
            transform.LookAt(other.transform.position);

            // Désactiver les controles & réactiver la souris
            other.GetComponent<PlayerInput>().actions.Disable();
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
