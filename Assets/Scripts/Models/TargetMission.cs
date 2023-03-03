using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetMission
{
    [SerializeField] private string name;
    [SerializeField] private GameObject triggerObject;
    [SerializeField] private EventType eventType;
    [SerializeField] private int scenarioNodeCode;

    public string GetName()
    {
        return name;
    }

    public GameObject GetTriggerObject()
    {
        return triggerObject;
    }

    public EventType GetEventType()
    {
        return eventType;
    }

    public int GetScenarioNodeCode()
    {
        return scenarioNodeCode;
    }
}
