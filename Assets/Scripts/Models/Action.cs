using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action : EventNode
{
    [TextArea(3,10)] [SerializeField] private List<string> choices;
    [SerializeField] private List<int> scenarioNodeNextCodes;
    [SerializeField] private int choosenActionIndex;

    public List<string> GetChoices() {
        return choices;
    }

    public List<int> GetScenarioNodeNextCodes() {
        return scenarioNodeNextCodes;
    }

    public int GetChoosenActionIndex()
    {
        return choosenActionIndex;
    }

    public void SetChoosenActionIndex(int choosenActionIndex)
    {
        this.choosenActionIndex = choosenActionIndex;
    }
}
