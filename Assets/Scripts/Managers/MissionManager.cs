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
    private TriggerEvent triggerEvent;

    // UI
    public GameObject missionCanvas;
    public TextMeshProUGUI mainText;

    public List<Mission> missions;
    public Dictionary<int, Mission> missionDictionary;

    private Mission currentMission;
    private int indexMission;

    private void Awake()
    {
        missionDictionary = new Dictionary<int, Mission>();
        foreach (Mission mission in missions)
        {
            missionDictionary.Add(mission.code, mission);
        }
    }

    public void LoadMission(int missionCode) {
        currentMission = FindMissionByCode(missionCode);
        mainText.text = currentMission.GetMainText();

        // Ajouter le composant TriggerEvent à l'objet qui déclenchera l'événement onTriggerEnter
        triggerEvent = currentMission.GetTriggerObject().AddComponent<TriggerEvent>();
        EnableTriggerEvent();
    }

    void OnTriggerEnterEvent(Collider other)
    {
        // Vérifier si le joueur est entré en collision avec l'objet spécifié
        if (other.gameObject == player)
        {
            EndMission();
        }
    }

    void DisableTriggerEvent()
    {
        // Désactiver l'événement onTriggerEnter
        triggerEvent.onTriggerEnterEvent.RemoveAllListeners();
    }

    void EnableTriggerEvent()
    {
        // Réactiver l'événement onTriggerEnter
        triggerEvent.onTriggerEnterEvent.AddListener(OnTriggerEnterEvent);
    }

    public void StartMission()
    {
        missionCanvas.SetActive(true);
    }

    public void EndMission()
    {
        DisableTriggerEvent();
        missionCanvas.SetActive(false);
        gameManager.updateProgression(currentMission.nextScenarioNodeCode);
    }

    public Mission FindMissionByCode(int missionCode) {
        if (missionDictionary.ContainsKey(missionCode))
        {
            return missionDictionary[missionCode];
        }
        else
        {
            Debug.LogError("Code de Mission invalide");
            return null;
        }
    }
}
