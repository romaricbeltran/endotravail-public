using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MissionAction", menuName = "ScriptableObjects/Actions/MissionAction" )]
public class MissionAction : BaseAction
{
	[SerializeField] [TextArea] private string title;
	[SerializeField] [TextArea] private string description;
	[SerializeField] private int managerTargetMissionListID;

    public global::System.String Title { get => title; set => title = value; }
    public global::System.String Description { get => description; set => description = value; }
    public global::System.Int32 ManagerTargetMissionListID { get => managerTargetMissionListID; set => managerTargetMissionListID = value; }
}
