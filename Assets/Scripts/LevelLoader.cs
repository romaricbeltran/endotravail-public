using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public TextMeshProUGUI levelTitleText;
    public Slider loadingBar;
    public TextMeshProUGUI loadingBarProgressText;

    // CrossFade transition
    public Animator loadingScreenTransition;
    public float transitionTime = 5.0f;
    public GameObject retardedUI;

    public void LoadLevel(int sceneIndex)
    {
        // Changement du texte en fonction de la scène chargée
        switch (sceneIndex)
        {
            case 0:
                levelTitleText.text = "Début du jeu";
                break;
            case 1:
                levelTitleText.text = "Chapitre 1";
                break;
            case 2:
                levelTitleText.text = "Chapitre 2";
                break;
            case 3:
                levelTitleText.text = "Chapitre 3";
                break;
            case 4:
                levelTitleText.text = "Fin du jeu";
                break;
            default:
                levelTitleText.text = null;
                break;
        }

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
            loadingBarProgressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}
