using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Gère les dialogues pour une scène
public class DialogueManager : MonoBehaviour
{
    public GameManager gameManager;
    public MissionManager missionManager;
    public GameObject player;
    public Animator dialogueBoxAnimator;
    public AudioSource audioSource;
    
    // UI
    public GameObject dialogueCanvas;
    public Image characterImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public float typingLetterInterval = 0.05f;
    public float timeBeetweenSentencesInterval = 3.0f;

    // Liste des dialogues de la scène
    public List<Dialogue> dialogues;
    public Dictionary<int, Dialogue> dialogueDictionary;

    private Dialogue currentDialogue;

    private bool isSentenceWritingFinished = false;
    private bool isAudioPlayingFinished = false;

    // Liste des phrases d'un dialogue (nom + texte)
    private List<Sentence> sentences;

    private int indexDialogue;
    private int indexSentence;

    private void Awake()
    {
        dialogueDictionary = new Dictionary<int, Dialogue>();
        foreach (Dialogue dialogue in dialogues)
        {
            dialogueDictionary.Add(dialogue.code, dialogue);
        }
    }

    // On précharge le dialogue avec la première phrase
    public void LoadDialogue(int dialogueCode) {
        // Close in case we skipped dialogue trigger
        dialogueBoxAnimator.SetBool("IsOpen", false);
        audioSource.Stop();
        StopAllCoroutines();

        currentDialogue = FindDialogueByCode(dialogueCode);
        indexSentence = 0;
        sentences = currentDialogue.GetSentences();
    }

    public void StartDialogue() {
        dialogueBoxAnimator.SetBool("IsOpen", true);
        DisplayNextSentence();
    }

    public void EndDialogue() {
        dialogueBoxAnimator.SetBool("IsOpen", false);
        StopAllCoroutines();

        if (MissionManager.ON_MISSION_END)
        {
            missionManager.EndMission();
        }
        else
        {
            gameManager.updateProgression(currentDialogue.nextScenarioNodeCode);
        }
    }

    public bool HasNextSentence()
    {
        return indexSentence >= 0 && indexSentence < sentences.Count;
    }

    public void DisplayNextSentence()
    {
        StopAllCoroutines();
        isAudioPlayingFinished = false;        
        isSentenceWritingFinished = false;

        if (HasNextSentence())
        {
            nameText.text = sentences[indexSentence].GetCharacter();
            characterImage.sprite = sentences[indexSentence].GetCharacterImage();
            dialogueText.text = sentences[indexSentence].GetText();
            audioSource.clip = sentences[indexSentence].GetAudioClip();
            StartCoroutine(TypeSentence(sentences[indexSentence].GetText()));
            StartCoroutine(PlayAudio());
            indexSentence++;
        } else
        {
            EndDialogue();
        }
    }

    IEnumerator LoadAudioClip(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error while downloading audio clip : " + www.error);
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                if (audioClip != null)
                {
                    audioSource.clip = audioClip;
                }
            }
        }
    }

    IEnumerator PlayAudio()
    {
        if (audioSource.clip)
        {
            //Debug.Log("Playing audio clip : " + audioSource.clip);
            audioSource.Play();
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }

        isAudioPlayingFinished = true;
        StartCoroutine(CheckIfWeCanDisplayNextSentence());
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingLetterInterval);
        }
        isSentenceWritingFinished = true;
        StartCoroutine(CheckIfWeCanDisplayNextSentence());
    }

    IEnumerator CheckIfWeCanDisplayNextSentence()
    {
        if (isSentenceWritingFinished && isAudioPlayingFinished)
        {
            yield return new WaitForSeconds(timeBeetweenSentencesInterval);
            DisplayNextSentence();
        }
    }

    public Dialogue FindDialogueByCode(int dialogueCode) {
        if (dialogueDictionary.ContainsKey(dialogueCode))
        {
            return dialogueDictionary[dialogueCode];
        }
        else
        {
            Debug.LogError("Code de Dialogue invalide");
            return null;
        }
    }
}
