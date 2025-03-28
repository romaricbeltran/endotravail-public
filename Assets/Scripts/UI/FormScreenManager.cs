using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;

public class FormScreenManager : MonoBehaviour
{
    public string serverRequestURL = " ";
    public string formId = "";
    public Button[] starButtons;
    public Sprite starOnSprite;
    public Sprite starOffSprite;
    private int satisfactionLevel = 0;
    public TMP_InputField improvementInputField;
    public TMP_InputField reviewInputField;
    public Button submitButton;
    public TextMeshProUGUI submitButtonText;
    public GameObject errorText;

    private void Start()
    {
        UpdateStarUI();
    }

    public void HandlePointerEnter(int starIndex)
    {
        for (int i = 0; i < starButtons.Length; i++)
        {
            Image starImage = starButtons[i].GetComponent<Image>();
            starImage.sprite = (i < starIndex) ? starOnSprite : starOffSprite;
        }
    }

    public void HandlePointerExit(int starIndex)
    {
        UpdateStarUI();
    }

    void UpdateStarUI()
    {
        for (int i = 0; i < starButtons.Length; i++)
        {
            Image starImage = starButtons[i].GetComponent<Image>();
            starImage.sprite = (i < satisfactionLevel) ? starOnSprite : starOffSprite;
        }
    }

    public void HandleStarClick(int starIndex)
    {
        satisfactionLevel = starIndex;
        UpdateStarUI();
    }

    public void SubmitForm()
    {
        if (satisfactionLevel != 0) {
            StartCoroutine(PostForm());
            submitButton.interactable = false;
        } else {
            errorText.SetActive(true);
        }
    }

    private IEnumerator PostForm()
    {
        WWWForm form = new WWWForm();
        form.AddField("form_id", formId);
        form.AddField("submitted[new_1705848294337]", satisfactionLevel);
        form.AddField("submitted[new_1705848622084]", improvementInputField.text);
        form.AddField("submitted[new_1705503390549]", reviewInputField.text);

        UnityWebRequest www = UnityWebRequest.Post(serverRequestURL, form);
        submitButtonText.text = "Envoi en cours...";
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
        
        LevelLoader.LoadEnd();
    }
}
