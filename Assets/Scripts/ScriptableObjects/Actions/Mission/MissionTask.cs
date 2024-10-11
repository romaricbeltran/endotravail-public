using UnityEngine;

[System.Serializable]
public class MissionTask : ScriptableObject
{
	[SerializeField] private string missionTaskName;
	[SerializeField] private GameObject triggerTaskObject;
	[SerializeField] private BaseAction action;
}
