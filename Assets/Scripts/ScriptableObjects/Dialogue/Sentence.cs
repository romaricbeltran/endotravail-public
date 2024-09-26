using UnityEngine;

[System.Serializable]
public class Sentence
{
    [SerializeField] private Character character;
    [SerializeField] private AudioClip audioClip;
    [TextArea(3,10)] [SerializeField] private string text;

    public Character Character { get => character; set => character = value; }
    public AudioClip AudioClip { get => audioClip; set => audioClip = value; }
    public global::System.String Text { get => text; set => text = value; }
}
