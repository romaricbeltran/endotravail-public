using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissionTrigger : MonoBehaviour
{
    public MissionManager missionManager;
    public ScenarioCode scenarioCode;

    public void TriggerMission()
    {
        missionManager.StartMission(scenarioCode);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerMission();
            // // Tourner le PNJ vers le player
            // transform.LookAt(other.transform.position);

            // // D�sactiver les controles & r�activer la souris
            // other.GetComponent<PlayerInput>().actions.Disable();
            // Cursor.lockState = CursorLockMode.None;
        }
    }
}
