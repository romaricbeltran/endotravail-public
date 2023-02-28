using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimelineTrigger : MonoBehaviour
{
    public TimelineManager timelineManager;
    public int scenarioNodeCode;

    public void TriggerTimeline()
    {
        timelineManager.PlayScenario(scenarioNodeCode);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerTimeline();

            // // D�sactiver les controles & r�activer la souris
            // other.GetComponent<PlayerInput>().actions.Disable();
            // Cursor.lockState = CursorLockMode.None;
        }
    }
}
