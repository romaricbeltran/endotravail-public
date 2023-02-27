using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @characterName : Nom du personnage qui prend la parole
// @sentences : Phrases du personnage qui prend la parole
[System.Serializable]
public class Monologue
{
    [SerializeField] private string character;
    [TextArea(3,10)] [SerializeField] private List<string> sentences;

    public string GetCharacter()
    {
        return character;
    }

    public List<string> GetSentences()
    {
        return sentences;
    }
}
