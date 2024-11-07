using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ChoiceAction", menuName = "ScriptableObjects/Actions/ChoiceAction" )]
public class ChoiceAction : BaseAction
{
	[SerializeField] private List<Choice> choices;

    public List<Choice> Choices { get => choices; set => choices = value; }
}

[System.Serializable]
public class Choice
{
	[SerializeField][TextArea( 3, 10 )] private string description;
	[SerializeField] private List<Flag> flags;
	[SerializeField] private List<Flag> showChoiceFlags;
	[SerializeField] private List<Flag> hideChoiceFlags;
	[SerializeField] private bool activateBackToMissionPOV;
	[SerializeField] private string nodeName;

    public global::System.String Description { get => description; set => description = value; }
    public List<Flag> Flags { get => flags; set => flags = value; }
	public List<Flag> ShowChoiceFlags { get => showChoiceFlags; set => showChoiceFlags = value; }
	public List<Flag> HideChoiceFlags { get => hideChoiceFlags; set => hideChoiceFlags = value; }
	public bool ActivateBackToMissionPOV { get => activateBackToMissionPOV; set => activateBackToMissionPOV = value; }
    public global::System.String NodeName { get => nodeName; set => nodeName = value; }
}
