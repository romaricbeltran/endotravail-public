using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueAction", menuName = "ScriptableObjects/Actions/DialogueAction" )]
public class DialogueAction : BaseAction
{
	[SerializeField] private List<Sentence> sentences;

    public List<Sentence> Sentences { get => sentences; set => sentences = value; }
}

[System.Serializable]
public class Sentence
{
	[SerializeField] private Character character;
	[SerializeField] private AudioClip audioClip;
	[TextArea( 3, 10 )][SerializeField] private string text;

	public Character Character { get => character; set => character = value; }
	public AudioClip AudioClip { get => audioClip; set => audioClip = value; }
	public global::System.String Text { get => text; set => text = value; }
}
