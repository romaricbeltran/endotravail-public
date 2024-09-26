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
    public TextMeshProUGUI waitLoadingText;

    // CrossFade transition
    public Animator loadingScreenTransition;
    public float creditTime = 4.5f;
    public float transitionTime = 0.1f;
    public GameObject retardedUI;

    // Home Screen Transition
    public Animator homeScreenTransition;

	private AudioSource audioTheme;

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

	private void Start()
	{
		audioTheme = GetComponent<AudioSource>();
    }

    public static void SaveProgress(int levelIndex)
    {
        PlayerPrefs.SetInt(PLAYER_PROGRESS, levelIndex);
        PlayerPrefs.Save();
    }

    public static int LoadProgress()
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
				levelTitleText.text = "CHAPITRE 2";
				levelSubtitleText.text = "Au bureau";
				StartCoroutine(LoadAddressableAsynchronously(addressIndex-2));
                break;
            // Chapitre 3
            case 4:
				levelTitleText.text = "CHAPITRE 3";
				levelSubtitleText.text = "L'annonce";
				StartCoroutine(LoadAddressableAsynchronously(addressIndex-2));
                break;
            // Chapitre 4
            case 5:
				levelTitleText.text = "CHAPITRE 4";
				levelSubtitleText.text = "L'aménagement";
				StartCoroutine(LoadAddressableAsynchronously(addressIndex-2));
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
    }

    IEnumerator LoadNonAddressableAsynchronously(int sceneIndex)
    {
        // Wait for Credits (homeAnimation)
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            yield return new WaitForSeconds(creditTime);
        }

		// Start CrossFade animation
		loadingScreenTransition.SetBool( "Display", true );
		yield return new WaitForSeconds( transitionTime );
		retardedUI.SetActive(true);

		if ( !audioTheme.isPlaying )
		{
			audioTheme.Play();
		}

		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.value = progress;

            yield return null;
        }

		retardedUI.SetActive( false );
		loadingScreenTransition.SetBool( "Display", false );
	}

    IEnumerator LoadAddressableAsynchronously(int addressIndex)
    {
		// Start CrossFade animation
		loadingScreenTransition.SetBool( "Display", true );
        retardedUI.SetActive(true);
		yield return new WaitForSeconds( transitionTime );


		IEnumerator waitLoadingCoroutine = WaitLoadingCoroutine();
        StartCoroutine(waitLoadingCoroutine);
        
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

        StopCoroutine(waitLoadingCoroutine);
        waitLoadingText.text = "";

		retardedUI.SetActive( false );
		audioTheme.Stop();

		SceneInstance sceneInstance = loadingSceneOperation.Result;
        sceneInstance.ActivateAsync();

		loadingScreenTransition.SetBool( "Display", false );
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

    public IEnumerator WaitLoadingCoroutine()
    {
        float timer = 0;
        bool message30SecondsShown = false;
        bool message1MinuteShown = false;

        while (!message30SecondsShown || !message1MinuteShown)
        {
            timer += Time.deltaTime;

            if (timer > 30 && !message30SecondsShown)
            {
                waitLoadingText.text = "Le serious game peut prendre 1 à 2 minutes à charger, merci pour votre patience.";
                message30SecondsShown = true;
            }

            if (timer > 60 && !message1MinuteShown)
            {
                waitLoadingText.text = "Merci de patienter, le chargement est plus long que prévu en raison d'une faible connexion internet. Privilégiez une connexion Wifi.";
                message1MinuteShown = true;
            }

            yield return null;
        }
    }

    public void OpenLink(string openURL)
    {
        OpenLinks.OpenURL(openURL);
    }
}
