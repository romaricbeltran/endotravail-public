using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TimelineManager timelineManager;
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
		timelineManager.Play(); // Start Progression From Beginning
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
}
