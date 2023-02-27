using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public CinemachineVirtualCamera DesktopCamera;
    public CinemachineVirtualCamera playerFollowCamera;
    public CinemachineVirtualCamera sideViewCamera;
    public PlayableAsset[] timelineClips;

    private PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void SwitchTimelineClip()
    {
        switch (GameManager.CURRENT_SCENARIO_PROGRESS)
        {
            case ScenarioCode.Start:
                director.playableAsset = timelineClips[0];
                break;
            case ScenarioCode.Scene_1_Popup_1:
                director.playableAsset = timelineClips[1];
                director.Play();
                break;
            case ScenarioCode.Scene_1_Dialogue_1:
                director.playableAsset = timelineClips[2];
                director.Play();
                break;
            case ScenarioCode.Scene_1_Action_1:
                director.Stop();
                break;
            default:
                break;
        }
    }
}
