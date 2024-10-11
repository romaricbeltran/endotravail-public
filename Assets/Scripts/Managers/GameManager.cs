using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	public const string LEVEL_PROGRESS = "level_progress";

    public MyUICanvasControllerInput myUICanvasControllerInput;
    public GameObject analogicButtons;
    public GameObject player;

    private ThirdPersonController playerController;
    private PlayerInput playerInput;

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

	void Start()
    {
		playerController = player.GetComponent<ThirdPersonController>();
        playerInput = player.GetComponent<PlayerInput>();
    }

    public void SwitchPlayerInput(bool availability)
    {
        if (playerInput != null)
        {
            playerInput.enabled = availability;
        }
        
        EventSystem.current.SetSelectedGameObject(null);
        myUICanvasControllerInput.VirtualResetMove();
        analogicButtons.SetActive(availability);
    }

    public void ResetPlayerCamera(float targetRotationY)
    {
        playerController.SetCinemachineTargetYaw(targetRotationY);
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
