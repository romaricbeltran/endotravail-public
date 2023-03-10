using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission : EventNode
{
    [TextArea] [SerializeField] private string missionTitle;
    [TextArea] [SerializeField] private string missionText;
    [SerializeField] private List<TargetMission> targetMissions;
    [SerializeField] private bool isAccomplished;

    public string GetMissionTitle()
    {
        return missionTitle;
    }

    public string GetMissionText()
    {
        return missionText;
    }

    public List<TargetMission> GetTargetMissions()
    {
        return targetMissions;
    }

    public bool IsAccomplished()
    {
        return isAccomplished;
    }

    public void SetIsAccomplished(bool boolean)
    {
        isAccomplished = boolean;
    }
}
