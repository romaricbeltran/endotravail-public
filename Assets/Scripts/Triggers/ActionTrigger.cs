using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionTrigger : MonoBehaviour
{
    public ActionManager actionManager;

    public void TriggerAction()
    {
        actionManager.StartAction();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerAction();
            // // Tourner le PNJ vers le player
            // transform.LookAt(other.transform.position);

            // // D�sactiver les controles & r�activer la souris
            // other.GetComponent<PlayerInput>().actions.Disable();
            // Cursor.lockState = CursorLockMode.None;
        }
    }
}
