using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static string GAME_PROGRESSION;
    public TimelineManager timelineManager;
    public GameObject player;

    private PlayerInput playerInput;

    void Awake()
    {
        // Vérifie si une autre instance de GameManager existe déjà, dans ce cas détruit cet objet pour s'assurer qu'il n'y a qu'une seule instance
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // L'objet ne sera pas détruit lorsqu'une nouvelle scène sera chargée
        }
    }

    // GAME_PROGRESSION
    private void Start()
    {
        GAME_PROGRESSION = "Start";
        updateProgression(0); // Start Progression From Beginning
        playerInput = player.GetComponent<PlayerInput>();

    }

    public void updateProgression(int nodeCode) {
        timelineManager.PlayScenario(nodeCode);
    }

    public void SwitchPlayerInput(bool availability)
    {
        if (playerInput != null)
        {
            playerInput.enabled = availability;
        }
    }
}
