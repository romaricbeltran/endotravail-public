using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @characterName : Nom du personnage qui prend la parole
// @sentences : Phrases du personnage qui prend la parole
[System.Serializable]
public class Sentence
{
    [SerializeField] private string character;
    [TextArea(3,10)] [SerializeField] private string text;

    public string GetCharacter()
    {
        return character;
    }

    public string GetText()
    {
        return text;
    }
}
