using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sentence
{
    [SerializeField] private string character;
    [SerializeField] private Sprite characterImage;
    [SerializeField] private string audioClipURL;
    [TextArea(3,10)] [SerializeField] private string text;

    public string GetCharacter()
    {
        return character;
    }

    public Sprite GetCharacterImage()
    {
        return characterImage;
    }

    public string GetAudioClipURL()
    {
        return audioClipURL;
    }

    public string GetText()
    {
        return text;
    }
}
