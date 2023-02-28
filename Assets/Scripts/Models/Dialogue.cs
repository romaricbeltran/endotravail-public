using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @characterName : Nom du personnage qui prend la parole
// @sentences : Phrases du personnage qui prend la parole
[System.Serializable]
public class Dialogue
{
    public int nodeCode;
    public int nextNodeCode;
    [SerializeField] private List<Sentence> sentences;

    public List<Sentence> GetSentences()
    {
        return sentences;
    }
}
