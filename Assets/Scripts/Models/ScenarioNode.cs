using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum EventType {
    Popup,
    Dialogue,
    Mission,
    Action,
    Camera,
    End
}

[System.Serializable]
public class ScenarioNode
{
    public int scenarioNodeCode;
    [SerializeField] private string name;
    [SerializeField] private PlayableAsset timelineClip;
    [SerializeField] private DirectorWrapMode directorWrapMode;
    [SerializeField] private EventType eventType;
    [SerializeField] private int eventIndex;

    public string GetName()
    {
        return name;
    }

    public PlayableAsset GetTimelineClip()
    {
        return timelineClip;
    }

    public DirectorWrapMode GetDirectorWrapMode()
    {
        return directorWrapMode;
    }

    public EventType GetEventType()
    {
        return eventType;
    }

    public int GetEventIndex()
    {
        return eventIndex;
    }
}
