using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;

[CreateAssetMenu( fileName = "Chapter", menuName = "ScriptableObjects/Chapter" )]

public class Chapter : ScriptableObject
{
	[SerializeField] private int id;
	[SerializeField] private List<ScenarioNode> scenario;

	public int Id { get => id; set => id = value; }
	public List<ScenarioNode> Scenario { get => scenario; set => scenario = value; }
}
