using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrientationManager : MonoBehaviour
{
    public static ScreenOrientationManager instance;
    public GameObject screenOrientation;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
		DetectRotation();
    }

    void Update()
    {
		DetectRotation();
    }

    void DetectRotation()
    {
        ScreenOrientation currentOrientation = Screen.orientation;

        if (currentOrientation != 0)
        {
            CanvasGroup canvasGroup = screenOrientation.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                switch (currentOrientation)
                {
                    case ScreenOrientation.Portrait:
                    case ScreenOrientation.PortraitUpsideDown:
                        canvasGroup.alpha = 1f;
                        canvasGroup.blocksRaycasts = true;
                        break;
                    case ScreenOrientation.LandscapeLeft:
                    case ScreenOrientation.LandscapeRight:
                        canvasGroup.alpha = 0f;
                        canvasGroup.blocksRaycasts = false;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Debug.LogWarning("Le composant CanvasGroup n'a pas été trouvé sur l'objet.");
            }
        }
    }
}
