using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sentence
{
    [SerializeField] private string character;
    [SerializeField] private Sprite characterImage;
    [SerializeField] private AudioClip audioClip;
    [TextArea(3,10)] [SerializeField] private string text;

    public string GetCharacter()
    {
        return character;
    }

    public Sprite GetCharacterImage()
    {
        return characterImage;
    }

    public AudioClip GetAudioClip()
    {
        return audioClip;
    }

    public string GetText()
    {
        return text;
    }
}
