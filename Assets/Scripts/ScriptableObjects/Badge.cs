using UnityEngine;

[CreateAssetMenu(fileName = "Badge", menuName = "ScriptableObjects/Badge" )]
public class Badge : ScriptableObject
{
	[SerializeField] private string badgeName;
    [SerializeField] private bool isUnlocked;

    public global::System.String BadgeName { get => badgeName; set => badgeName = value; }
    public global::System.Boolean IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
}
