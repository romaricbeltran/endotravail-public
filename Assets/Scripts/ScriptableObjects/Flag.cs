using UnityEngine;

[CreateAssetMenu(fileName = "Flag", menuName = "ScriptableObjects/Flag" )]
public class Flag : ScriptableObject
{
	[SerializeField] private string flagName;
	[SerializeField] private bool persistedInProgress;

	public global::System.String FlagName { get => flagName; set => flagName = value; }
    public global::System.Boolean PersistedInProgress { get => persistedInProgress; set => persistedInProgress = value; }
}
