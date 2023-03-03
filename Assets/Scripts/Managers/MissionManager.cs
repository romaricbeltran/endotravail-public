using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;


// Au lancement d'une mission, on crée dynamiquement des composants TriggerEvent sur tous les GameObjects TargetMission concernés.
// On active tous les écouteurs qui pointent sur les TriggerEvent de nos TargetMission.
// Lorsqu'un trigger est déclenché on détruit l'écouteur vers le TriggerEvent concerné.
// Si c'était le dernier écouteur, on set ON_MISSION_END à true pour lancer EndMission sur un prochain call de Manager.
public class MissionManager : MonoBehaviour
{
    public GameManager gameManager;
    public TimelineManager timelineManager;
    public GameObject player;
    public static bool ON_MISSION_END;

    public List<TriggerEvent> triggerEvents;
    private List<TargetMission> currentTargetMissions;
    private int currentTriggerMissionIndex;

    // UI
    public GameObject missionCanvas;
    public GameObject shadowBackground;
    public GameObject missionBox;
    public TextMeshProUGUI missionTitle;
    public TextMeshProUGUI missionText;
    public Button acceptButton;

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

    // On va charger la mission courante, assigner et écouter tous les TriggerEvents
    public void LoadMission(int missionCode) {
        currentMission = FindMissionByCode(missionCode);
        currentTargetMissions = currentMission.GetTargetMissions();
        missionTitle.text = currentMission.GetMissionTitle();
        missionText.text = currentMission.GetMissionText();

        int indexMission = 0;

        foreach (TargetMission targetMission in currentTargetMissions)
        {
            TriggerEvent newTriggerEvent = targetMission.GetTriggerObject().AddComponent<TriggerEvent>();
            // Ajouter aussi l'index de la mission pour pouvoir l'identifier au trigger
            newTriggerEvent.SetIndexMission(indexMission);
            indexMission++;
            
            AddTriggerEvent(newTriggerEvent);
            Debug.Log("Add Trigger Event for Target Mission:" + targetMission.GetName() + " on " + targetMission.GetTriggerObject());
        }
    }

    // Lorsqu'on trigger un TriggerEvent, on pointe sur le scénario node cible du TriggerMission
    // Puis on détruit l'écouteur du TriggerEvent (pourquoi pas détruire directement le composant plutot ? au cas où on a besoin de le réactiver ?)
    void OnTriggerEnterEvent(Tuple<GameObject, Collider> args)
    {   
        // Récupère l'objet qui porte le trigger
        GameObject sender = args.Item1;
        Collider other = args.Item2;

        // Vérifier si le joueur est entré en collision avec l'objet spécifié
        if (other.gameObject == player)
        {
            TriggerEvent currentTriggerEvent = sender.GetComponent<TriggerEvent>();
            int currentTriggerMissionIndex = currentTriggerEvent.GetIndexMission();

            gameManager.updateProgression(currentTargetMissions[currentTriggerMissionIndex].GetScenarioNodeCode());
            
            RemoveTriggerEvent(currentTriggerEvent);
            Debug.Log("Remove Trigger Event for Target Mission:" + currentTargetMissions[currentTriggerMissionIndex].GetName() + " on " + other.gameObject.name);

            if (triggerEvents.Count == 0)
            {
                ON_MISSION_END = true;
            }
        }
    }

    void AddTriggerEvent(TriggerEvent triggerEvent)
    {
        triggerEvent.onTriggerEnterEvent.AddListener(OnTriggerEnterEvent);
        triggerEvents.Add(triggerEvent);
    }

    void RemoveTriggerEvent(TriggerEvent triggerEvent)
    {
        triggerEvent.onTriggerEnterEvent.RemoveAllListeners();
        triggerEvents.Remove(triggerEvent);
    }

    public void StartMission()
    {
        shadowBackground.SetActive(true);
        missionBox.GetComponent<RectTransform>().sizeDelta = new Vector2(missionBox.GetComponent<RectTransform>().sizeDelta.x, 200f);
        acceptButton.gameObject.SetActive(true);
        missionCanvas.SetActive(true);
    }

    public void AcceptMission()
    {
        acceptButton.gameObject.SetActive(false);
        missionBox.GetComponent<RectTransform>().sizeDelta = new Vector2(missionBox.GetComponent<RectTransform>().sizeDelta.x, 115f);
        shadowBackground.SetActive(false);
        gameManager.SwitchPlayerInput(true);
    }

    // A la fin de la mission on lance le node qui suit la fin de la mission ! (entre les deux on a les nodes des targetMissions)
    public void EndMission()
    {
        missionCanvas.SetActive(false);
        gameManager.updateProgression(currentMission.nextScenarioNodeCode);
        ON_MISSION_END = false;
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
