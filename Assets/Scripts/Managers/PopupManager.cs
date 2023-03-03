using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public GameManager gameManager;
    public MissionManager missionManager;

    // UI
    public GameObject popupCanvas;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI sourceText;
    public Button nextButton;

    public List<Popup> popups;
    public Dictionary<int, Popup> popupDictionary;

    private Popup currentPopup;
    private int indexPopup;

    private void Awake()
    {
        popupDictionary = new Dictionary<int, Popup>();
        foreach (Popup popup in popups)
        {
            popupDictionary.Add(popup.code, popup);
        }
    }

    public void LoadPopup(int popupCode) {
        currentPopup = FindPopupByCode(popupCode);
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

        if (MissionManager.ON_MISSION_END)
        {
            missionManager.EndMission();
        }
        else
        {
            gameManager.updateProgression(currentPopup.nextScenarioNodeCode);
        }
    }

    public Popup FindPopupByCode(int popupCode) {
        if (popupDictionary.ContainsKey(popupCode))
        {
            return popupDictionary[popupCode];
        }
        else
        {
            Debug.LogError("Code de Popup invalide");
            return null;
        }
    }
}
