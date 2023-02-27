using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimelineTrigger : MonoBehaviour
{
    public TimelineManager timelineManager;

    public void TriggerTimeline()
    {
        timelineManager.SwitchTimelineClip();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.CURRENT_SCENARIO_PROGRESS = ScenarioCode.Scene_1_Dialogue_1;
            TriggerTimeline();
            // // Tourner le PNJ vers le player
            transform.LookAt(other.transform.position);

            // // D�sactiver les controles & r�activer la souris
            // other.GetComponent<PlayerInput>().actions.Disable();
            // Cursor.lockState = CursorLockMode.None;
        }
    }
}
