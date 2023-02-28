using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MissionManager : MonoBehaviour
{
    public GameManager gameManager;
    public TimelineManager timelineManager;
    public GameObject player;

    // UI
    public GameObject missionCanvas;
    public TextMeshProUGUI mainText;

    public List<Mission> missions;
    private Mission currentMission;
    private int indexMission;

    public void LoadMission(int missionCode) {
        idiotSearchMission(missionCode);
        mainText.text = missions[indexMission].GetMainText();
    }

    public void StartMission()
    {
        missionCanvas.SetActive(true);
    }

    public void EndMission()
    {
        missionCanvas.SetActive(false);
        gameManager.updateProgression(currentMission.nextNodeCode);
    }

    // Les éléments doivent être dans l'ordre (plus performant que Find ou de faire un map)
    public void idiotSearchMission(int missionCode) {
        currentMission = missions[0];
    }
}
