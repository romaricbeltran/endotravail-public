using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PopupTrigger : MonoBehaviour
{
    public PopupManager popupManager;
    public ScenarioCode scenarioCode;

    public void TriggerPopup()
    {
        popupManager.StartPopup(scenarioCode);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerPopup();
            // // Tourner le PNJ vers le player
            // transform.LookAt(other.transform.position);

            // // D�sactiver les controles & r�activer la souris
            // other.GetComponent<PlayerInput>().actions.Disable();
            // Cursor.lockState = CursorLockMode.None;
        }
    }
}
