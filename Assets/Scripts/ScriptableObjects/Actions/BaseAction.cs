using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

[System.Serializable]
public class BaseAction : ScriptableObject
{
	[SerializeField] private PlayableAsset timelineClip;
	[SerializeField] private DirectorWrapMode directorWrapMode;

    public PlayableAsset TimelineClip { get => timelineClip; set => timelineClip = value; }
    public DirectorWrapMode DirectorWrapMode { get => directorWrapMode; set => directorWrapMode = value; }
}
