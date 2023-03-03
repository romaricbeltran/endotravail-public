using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission : EventNode
{
    [TextArea] [SerializeField] private string mainText;
    [SerializeField] private List<TargetMission> targetMissions;
    [SerializeField] private bool isAccomplished;

    public string GetMainText()
    {
        return mainText;
    }

    public List<TargetMission> GetTargetMissions()
    {
        return targetMissions;
    }

    public bool IsAccomplished()
    {
        return isAccomplished;
    }
}
