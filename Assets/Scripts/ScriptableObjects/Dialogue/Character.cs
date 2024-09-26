using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Dialogue/Character" )]
public class Character : ScriptableObject
{
	[SerializeField] private string characterName;
	[SerializeField] private Sprite characterImage;

    public global::System.String CharacterName { get => characterName; set => characterName = value; }
    public Sprite CharacterImage { get => characterImage; set => characterImage = value; }
}
