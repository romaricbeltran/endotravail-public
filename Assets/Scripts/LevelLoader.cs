using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    // Tableau contenant les adresses des scènes, indexé par numéro de scène
    public string[] sceneAddresses;

    public TextMeshProUGUI levelTitleText;
    public TextMeshProUGUI levelSubtitleText;
    public Slider loadingBar;

    // CrossFade transition
    public Animator loadingScreenTransition;
    public float transitionTime = 3.0f;
    public GameObject retardedUI;

    // Home Screen Transition
    public Animator homeScreenTransition;

    public void ResetGame() {
        StartCoroutine(LoadNonAdressableAsynchronously(0));
    }

    public void FirstLevel()
    {
        homeScreenTransition.SetTrigger("Start");
        StartCoroutine(LoadFirstLevel());
    }

    IEnumerator LoadFirstLevel()
    {
        // Start CrossFade animation
        yield return new WaitForSeconds(transitionTime*1.5f);
        LoadLevel(0);
    }

    public void LoadLevel(int addressIndex)
    {
        // Changement du texte en fonction de la scène chargée
        switch (addressIndex)
        {
            case 0:
                levelTitleText.text = "CHAPITRE 1";
                levelSubtitleText.text = "L'absence";
                break;
            case 1:
                levelTitleText.text = "CHAPITRE 2";
                levelSubtitleText.text = "Une journée type dans la peau d'une personne atteinte d'endométriose";
                break;
            case 2:
                levelTitleText.text = "CHAPITRE 3";
                levelSubtitleText.text = "L'annonce au manager";
                break;
            case 3:
                levelTitleText.text = "CHAPITRE 4";
                levelSubtitleText.text = "Le rendez-vous avec la médecine du travail";
                break;
            case 4:
                levelTitleText.text = "";
                break;
            default:
                levelTitleText.text = null;
                break;
        }

        GameManager.GAME_PROGRESSION = levelTitleText.text;
        StartCoroutine(LoadAddressableAsynchronously(addressIndex));
    }

    IEnumerator LoadNonAdressableAsynchronously(int sceneIndex)
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

    IEnumerator LoadAddressableLabelAsynchronously(string labelName)
    {
        AsyncOperationHandle<IList<GameObject>> loadingAssetOperation = Addressables.LoadAssetsAsync<GameObject>(labelName, null);
        Debug.Log("Loading Assets Label " + labelName);

        while (!loadingAssetOperation.IsDone)
        {
            float progress = Mathf.Clamp01(loadingAssetOperation.PercentComplete);
            //Debug.Log(loadingAssetOperation.PercentComplete);
            loadingBar.value = progress;

            yield return null;
        }

        yield return loadingAssetOperation;
    }

    IEnumerator LoadAddressableAsynchronously(int addressIndex)
    {
        // Start CrossFade animation
        loadingScreenTransition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        retardedUI.SetActive(true);
        
        switch (addressIndex)
        {
            case 0:
                yield return StartCoroutine(LoadAddressableLabelAsynchronously("Chapitre1"));
                yield return StartCoroutine(LoadAddressableLabelAsynchronously("DuplicateAssets"));
                break;
            case 1:
                yield return StartCoroutine(LoadAddressableLabelAsynchronously("Chapitre2"));
                break;
            case 2:
                yield return StartCoroutine(LoadAddressableLabelAsynchronously("Chapitre3"));
                break;
            case 3:
                yield return StartCoroutine(LoadAddressableLabelAsynchronously("Chapitre4"));
                break;
            default:
                break;
        }

        AsyncOperationHandle<SceneInstance> loadingSceneOperation = Addressables.LoadSceneAsync(sceneAddresses[addressIndex], LoadSceneMode.Single, false);
        Debug.Log("Loading Scene " + addressIndex);

        while (!loadingSceneOperation.IsDone)
        {
            float progress = Mathf.Clamp01(loadingSceneOperation.PercentComplete);
            //Debug.Log(loadingSceneOperation.PercentComplete);
            loadingBar.value = progress;

            yield return null;
        }

        Debug.Log("Loading Complete");

        SceneInstance sceneInstance = loadingSceneOperation.Result;
        sceneInstance.ActivateAsync();
    }

    public void OpenLink(string openURL)
    {
        OpenLinks.OpenURL(openURL);
    }
}
