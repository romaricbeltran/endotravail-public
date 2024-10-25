using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	public const string LEVEL_PROGRESS = "level_progress";

    public GameObject analogicButtons;
    public GameObject player;

	void Awake()
	{
		if ( instance != null )
		{
			Destroy( gameObject );
		}
		else
		{
			instance = this;
			DontDestroyOnLoad( gameObject );
		}
	}

	public void SwitchPlayerInput(bool availability)
    {
        if ( player.GetComponent<PlayerInput>() != null)
        {
			player.GetComponent<PlayerInput>().enabled = availability;
        }

		if ( analogicButtons != null )
		{
			analogicButtons.SetActive(availability);
			analogicButtons.GetComponent<GraphicRaycaster>().enabled = availability;
		}

		EventSystem.current.SetSelectedGameObject(null);
		analogicButtons.GetComponent<MyUICanvasControllerInput>().VirtualResetMove();
    }

    public void ResetPlayerCamera(float targetRotationY)
    {
		player.GetComponent<ThirdPersonController>().SetCinemachineTargetYaw(targetRotationY);
    }

	public static void SaveProgress(int levelIndex)
	{
		PlayerPrefs.SetInt( LEVEL_PROGRESS, levelIndex );
		PlayerPrefs.Save();
	}

	public static int LoadProgress()
	{
		return PlayerPrefs.GetInt( LEVEL_PROGRESS, 0 );
	}

	public static void ResetProgress()
	{
		PlayerPrefs.DeleteAll();
	}
}
