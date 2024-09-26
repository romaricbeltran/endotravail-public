using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MissionScenarioNode", menuName = "ScriptableObjects/Mission/MissionScenarioNode" )]
public class MissionScenarioNode : BaseScenarioNode
{
	[SerializeField] [TextArea] private string missionTitle;
	[SerializeField] [TextArea] private string missionText;
	[SerializeField] private List<MissionTask> missionTasks;
	[SerializeField] private bool isAccomplished;

    public global::System.String MissionName { get => missionTitle; set => missionTitle = value; }
    public global::System.String MissionText { get => missionText; set => missionText = value; }
    public List<MissionTask> MissionTasks { get => missionTasks; set => missionTasks = value; }
    public global::System.Boolean IsAccomplished { get => isAccomplished; set => isAccomplished = value; }
}
