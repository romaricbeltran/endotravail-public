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
    public static LevelLoader instance;
    public const string PLAYER_PROGRESS = "player_progress";
    public GameManager gameManager;

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

    private void Start()
    {
        AudioSource audio = FindObjectOfType<AudioManager>().GetComponent<AudioSource>();
        
        if (SceneManager.GetActiveScene().buildIndex == -1)
        {
            audio.Stop();
        }
        else if (!audio.isPlaying)
        {
            audio.Play();
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            MenuScreenManager.EnableChapters();
        }
    }

    public void SaveProgress(int levelIndex)
    {
        PlayerPrefs.SetInt(PLAYER_PROGRESS, levelIndex);
        PlayerPrefs.Save();
    }

    public int LoadProgress()
    {
        return PlayerPrefs.GetInt(PLAYER_PROGRESS, 0);
    }

    public void LoadLevel(int addressIndex)
    {
        // Changement du texte en fonction de la scène chargée
        switch (addressIndex)
        {
            // Home
            case 0:
                levelTitleText.text = "";
                levelSubtitleText.text = "";
                StartCoroutine(LoadNonAddressableAsynchronously(0));
                break;
            // Menu
            case 1:
                levelTitleText.text = "";
                levelSubtitleText.text = "";
                if (SceneManager.GetActiveScene().buildIndex == 0) {
                    homeScreenTransition.SetTrigger("Start");
                }
                if (LoadProgress() == 0)
                {
                    SaveProgress(1);
                }
                StartCoroutine(LoadNonAddressableAsynchronously(1));
                break;
            // Chapitre 1
            case 2:
                levelTitleText.text = "CHAPITRE 1";
                levelSubtitleText.text = "L'absence";
                StartCoroutine(LoadAddressableAsynchronously(addressIndex-2));
                break;
            // Chapitre 2
            case 3:
                if (LoadProgress() > 1)
                {
                levelTitleText.text = "CHAPITRE 2";
                levelSubtitleText.text = "Au bureau";
                StartCoroutine(LoadAddressableAsynchronously(addressIndex-2));
                }
                break;
            // Chapitre 3
            case 4:
                if (LoadProgress() > 2)
                {
                levelTitleText.text = "CHAPITRE 3";
                levelSubtitleText.text = "L'annonce";
                StartCoroutine(LoadAddressableAsynchronously(addressIndex-2));
                
                }
                break;
            // Chapitre 4
            case 5:
                if (LoadProgress() > 3)
                {
                levelTitleText.text = "CHAPITRE 4";
                levelSubtitleText.text = "L'aménagement";
                StartCoroutine(LoadAddressableAsynchronously(addressIndex-2));
                }
                break;
            // Form review
            case 6:
                levelTitleText.text = "";
                StartCoroutine(LoadNonAddressableAsynchronously(2));
                break;
            // End
            case 7:
                levelTitleText.text = "";
                StartCoroutine(LoadNonAddressableAsynchronously(3));
                break;
            default:
                levelTitleText.text = null;
                break;
        }

        GameManager.GAME_PROGRESSION = levelTitleText.text;
    }

    IEnumerator LoadNonAddressableAsynchronously(int sceneIndex)
    {
        // Wait for Credits (homeAnimation)
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            yield return new WaitForSeconds(transitionTime * 1.5f);
        }

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
        Debug.Log("Loading Scene " + addressIndex+1);

        while (!loadingSceneOperation.IsDone)
        {
            float progress = Mathf.Clamp01(loadingSceneOperation.PercentComplete);
            //Debug.Log(loadingSceneOperation.PercentComplete);
            loadingBar.value = progress;

            yield return null;
        }

        Debug.Log("Scene Loading Complete");

        SceneInstance sceneInstance = loadingSceneOperation.Result;
        sceneInstance.ActivateAsync();
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

    public void OpenLink(string openURL)
    {
        OpenLinks.OpenURL(openURL);
    }
}
