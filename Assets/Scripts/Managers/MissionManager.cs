using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


// Au lancement d'une mission, on crée dynamiquement des composants TriggerEvent sur tous les GameObjects TargetMission concernés.
// On active tous les écouteurs qui pointent sur les TriggerEvent de nos TargetMission.
// Lorsqu'un trigger est déclenché on détruit l'écouteur vers le TriggerEvent concerné.
// Si c'était le dernier écouteur, on set ON_MISSION_END à true pour lancer EndMission sur un prochain call de Manager.
public class MissionManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    public static bool ON_MISSION_END;
    public static bool WAS_ACTION_MISSION_COMPONENT = false;

    public List<TriggerEvent> triggerEvents;
    private List<TargetMission> currentTargetMissions;
    private int currentTriggerMissionIndex;

    // UI
    public GameObject missionCanvas;
    public GameObject shadowBackground;
    public GameObject missionBox;
    public TextMeshProUGUI missionTitle;
    public TextMeshProUGUI missionText;
    public Button missionSkip;
    public Button acceptButton;

    public bool tutoOnFirstMission = false;

    public GameObject tutoAnalogicBox;
    public GameObject tutoSkipBox;
    
    public List<Mission> missions;
    public Dictionary<int, Mission> missionDictionary;

    private Mission currentMission;
    private int indexMission;
    private int skipTriggerIndex;

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
        skipTriggerIndex = 0;
        gameManager.SwitchPlayerInput(false);

        // Reinit UI ! Start_XXX never call on managers (exception for DialogueManager with a SignalEmitter)
        shadowBackground.SetActive(true);
        //missionBox.GetComponent<RectTransform>().sizeDelta = new Vector2(missionBox.GetComponent<RectTransform>().sizeDelta.x, 200f);
        acceptButton.gameObject.SetActive(true);
        missionSkip.gameObject.SetActive(false);
        
        indexMission = 0;

        foreach (TargetMission targetMission in currentTargetMissions)
        {
            TriggerEvent newTriggerEvent = targetMission.GetTriggerObject().AddComponent<TriggerEvent>();
            newTriggerEvent.SetIndexMission(indexMission);

            // Ajouter aussi l'index de la mission pour pouvoir l'identifier au trigger
            indexMission++;
            AddTriggerEvent(newTriggerEvent);
            //Debug.Log("Add Trigger Event for Target Mission:" + targetMission.GetName() + " on " + targetMission.GetTriggerObject());
        }
    }

    public void SkipTrigger()
    {
        WAS_ACTION_MISSION_COMPONENT = false;
        ON_MISSION_END = false;

        int indexToSkip = skipTriggerIndex;
        TriggerEvent currentTriggerEvent = currentTargetMissions[indexToSkip].GetTriggerObject().GetComponent<TriggerEvent>();
        int currentTriggerMissionIndex = currentTriggerEvent.GetIndexMission();

        RemoveTriggerEvent(currentTriggerEvent);
        Destroy(currentTriggerEvent);


        //Debug.Log("Remove Trigger Event for Target Mission:" + currentTargetMissions[currentTriggerMissionIndex].GetName() + " on " + currentTargetMissions[indexToSkip].GetTriggerObject().name);
        if (triggerEvents.Count == 0)
        {
            //Debug.Log("End of mission");
            missionCanvas.SetActive(false);
            currentMission.SetIsAccomplished(true);
            ON_MISSION_END = true;
        }
        
        skipTriggerIndex++;

        gameManager.updateProgression(currentTargetMissions[indexToSkip].GetScenarioNodeCode());
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

            RemoveTriggerEvent(currentTriggerEvent);
            Destroy(currentTriggerEvent);


            //Debug.Log("Remove Trigger Event for Target Mission:" + currentTargetMissions[currentTriggerMissionIndex].GetName() + " on " + other.gameObject.name);

            if (triggerEvents.Count == 0)
            {
                //Debug.Log("End of mission");
                missionCanvas.SetActive(false);
                currentMission.SetIsAccomplished(true);
                ON_MISSION_END = true;
            }

            gameManager.updateProgression(currentTargetMissions[currentTriggerMissionIndex].GetScenarioNodeCode());
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
        missionCanvas.SetActive(true);
    }

    public void AcceptMission()
    {
        acceptButton.gameObject.SetActive(false);
        missionSkip.gameObject.SetActive(true);
        if (tutoOnFirstMission)
        {
            StartTuto();
        }
        else
        {
            shadowBackground.SetActive(false);
            gameManager.SwitchPlayerInput(true);
        }
        // missionBox.GetComponent<RectTransform>().sizeDelta = new Vector2(missionBox.GetComponent<RectTransform>().sizeDelta.x, 130f);
    }

    public void StartTuto()
    {
        tutoAnalogicBox.SetActive(true);
        gameManager.analogicButtons.SetActive(true);
        gameManager.analogicButtons.GetComponent<GraphicRaycaster>().enabled = false;
        missionSkip.enabled = false;
    }

    public void NextTuto()
    {
        tutoAnalogicBox.SetActive(false);
        tutoSkipBox.SetActive(true);
    }

    public void EndTuto()
    {
        tutoSkipBox.SetActive(false);
        tutoAnalogicBox.SetActive(false);
        shadowBackground.SetActive(false);
        gameManager.SwitchPlayerInput(true);
        gameManager.analogicButtons.GetComponent<GraphicRaycaster>().enabled = true;
        missionSkip.enabled = true;
    }

    // A la fin de la mission on lance le node qui suit la fin de la mission ! (entre les deux on a les nodes des targetMissions)
    public void EndMission()
    {
        if (!WAS_ACTION_MISSION_COMPONENT)
        {
            gameManager.updateProgression(currentMission.nextScenarioNodeCode);  
        }

        WAS_ACTION_MISSION_COMPONENT = false;
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
