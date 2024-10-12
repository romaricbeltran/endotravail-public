using UnityEngine;

public class TriggerObjectListener : MonoBehaviour
{
	private MissionActionManager missionActionManager;
	private TargetMission targetMission;

	public void Initialize(MissionActionManager manager, TargetMission mission)
	{
		missionActionManager = manager;
		targetMission = mission;
	}

	private void OnTriggerEnter(Collider other)
	{
		if ( other.CompareTag( "Player" ) )
		{
			Debug.Log( $"Player collided with trigger: { targetMission.nodeName }" );
			missionActionManager.OnTriggerActivated( targetMission );
		}
	}
}
