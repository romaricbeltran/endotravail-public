using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action
{
    public int nodeCode;
    public int nextNodeCode;
    [TextArea(3,10)] [SerializeField] private string[] choices;
    [TextArea(3,10)] [SerializeField] private int choosenIndex;

    public string[] GetChoices() {
        return choices;
    }

    public int GetChoosenIndex() {
        return choosenIndex;
    }
}
