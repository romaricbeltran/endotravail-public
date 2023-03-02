using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue : EventNode
{
    [SerializeField] private List<Sentence> sentences;

    public List<Sentence> GetSentences()
    {
        return sentences;
    }
}
