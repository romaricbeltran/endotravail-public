using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class ScenarioNode
{
    public int code;
    public int nextCode;
    [SerializeField] private string name;
    [SerializeField] private PlayableAsset timelineClip;
    [SerializeField] private NodeEvent nodeEvent;

    public string GetName()
    {
        return name;
    }

    public PlayableAsset GetTimelineClip()
    {
        return timelineClip;
    }

    public NodeEvent GetNodeEvent()
    {
        return nodeEvent;
    }
}
