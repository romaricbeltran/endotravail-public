using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interactable
public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public ScenarioCode scenarioCode;

    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(scenarioCode);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue();
            // // Tourner le PNJ vers le player
            transform.LookAt(other.transform.position);

            // // D�sactiver les controles & r�activer la souris
            // other.GetComponent<PlayerInput>().actions.Disable();
            // Cursor.lockState = CursorLockMode.None;
        }
    }
}
