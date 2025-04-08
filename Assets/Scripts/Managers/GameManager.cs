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

    public GameObject joystickMobile;
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

        bool isMobile = Application.isMobilePlatform;

        if (isMobile && joystickMobile != null)
        {
	        joystickMobile.SetActive(availability);
	        joystickMobile.GetComponent<MyUICanvasControllerInput>().VirtualResetMove();
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ResetPlayerCamera(float targetRotationY)
    {
		player.GetComponent<MyThirdPersonController>().SetCinemachineTargetYaw(targetRotationY);
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
