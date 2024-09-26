using UnityEngine;

[System.Serializable]
public class MissionTask : ScriptableObject
{
	[SerializeField] private string missionTaskName;
	[SerializeField] private GameObject triggerTaskObject;
	[SerializeField] private BaseScenarioNode scenarioNode;

    public global::System.String MissionTaskName { get => missionTaskName; set => missionTaskName = value; }
    public GameObject TriggerTaskObject { get => triggerTaskObject; set => triggerTaskObject = value; }
    public BaseScenarioNode ScenarioNode { get => scenarioNode; set => scenarioNode = value; }
}
