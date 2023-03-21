using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public TextMeshProUGUI levelTitleText;
    public TextMeshProUGUI levelSubtitleText;
    public Slider loadingBar;

    // CrossFade transition
    public Animator loadingScreenTransition;
    public float transitionTime = 3.0f;
    public GameObject retardedUI;

    public void LoadLevel(int sceneIndex)
    {
        // Changement du texte en fonction de la scène chargée
        switch (sceneIndex)
        {
            case 0:
                levelTitleText.text = "";
                break;
            case 1:
                levelTitleText.text = "CHAPITRE 1";
                levelSubtitleText.text = "L'absence";
                break;
            case 2:
                levelTitleText.text = "CHAPITRE 2";
                levelSubtitleText.text = "Une journée type dans la peau d'une personne atteinte d'endométriose";
                break;
            case 3:
                levelTitleText.text = "CHAPITRE 3";
                levelSubtitleText.text = "L'annonce au manager";
                break;
            case 4:
                levelTitleText.text = "CHAPITRE 4";
                levelSubtitleText.text = "Le rendez-vous avec la médecine du travail";
                break;
            case 5:
                levelTitleText.text = "";
                break;
            default:
                levelTitleText.text = null;
                break;
        }

        GameManager.GAME_PROGRESSION = levelTitleText.text;
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        // Start CrossFade animation
        loadingScreenTransition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        retardedUI.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            
            loadingBar.value = progress;

            yield return null;
        }
    }

    public void OpenLink(string openURL)
    {
        Application.OpenURL(openURL);
    }
}
