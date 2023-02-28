using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public GameManager gameManager;

    // UI
    public GameObject popupCanvas;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI sourceText;
    public Button nextButton;

    public List<Popup> popups;
    private Popup currentPopup;
    private int indexPopup;

    public void LoadPopup(int popupCode) {
        idiotSearchPopup(popupCode);
        mainText.text = currentPopup.GetMainText();
        sourceText.text = currentPopup.GetSourceText();
    }

    public void StartPopup()
    {
        popupCanvas.SetActive(true);
    }

    public void EndPopup()
    {
        popupCanvas.SetActive(false);
        gameManager.updateProgression(2);
        // //Cursor.lockState = CursorLockMode.None;
    }

    // Les éléments doivent être dans l'ordre (plus performant que Find ou de faire un map)
    public void idiotSearchPopup(int popupCode) {
        currentPopup = popups[popupCode];
    }
}
