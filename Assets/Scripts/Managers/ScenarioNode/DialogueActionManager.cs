using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DialogueActionManager : BaseActionManager<DialogueAction>
{
    // UI
    public GameObject dialogueCanvas;
    public Image characterImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator dialogueBoxAnimator;
    public float typingLetterInterval = 0.05f;
    public float timeBeetweenSentencesInterval = 3.0f;

	public AudioSource audioSource;

    private List<Sentence> sentences;
    private int indexSentence;
    private bool isSentenceWritingFinished = false;
    private bool isAudioPlayingFinished = false;

    // On précharge le dialogue avec la première phrase
    public override void LoadData(DialogueAction currentAction) {
		// Close in case we skipped dialogue trigger
		dialogueBoxAnimator.SetBool("IsOpen", false);
        audioSource.Stop();
        StopAllCoroutines();

		indexSentence = 0;
        sentences = currentAction.Sentences;
	}

	public override void StartAction() {
        dialogueBoxAnimator.SetBool("IsOpen", true);
        DisplayNextSentence();
    }

    public override void EndAction() {
        dialogueBoxAnimator.SetBool("IsOpen", false);
        StopAllCoroutines();
		base.EndAction();
	}

    public void DisplayNextSentence()
    {
        StopAllCoroutines();
        isAudioPlayingFinished = false;        
        isSentenceWritingFinished = false;

        if (HasNextSentence())
        {
            nameText.text = sentences[indexSentence].Character.CharacterName;
            characterImage.sprite = sentences[indexSentence].Character.CharacterImage;
            audioSource.clip = sentences[indexSentence].AudioClip;
            dialogueText.text = sentences[indexSentence].Text;
            StartCoroutine(TypeSentence(sentences[indexSentence].Text));
            StartCoroutine(PlayAudio());
            indexSentence++;
        } else
        {
			EndAction();
		}
    }

	public bool HasNextSentence()
	{
		return indexSentence >= 0 && indexSentence < sentences.Count;
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach ( char letter in sentence.ToCharArray() )
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds( typingLetterInterval );
		}
		isSentenceWritingFinished = true;
		StartCoroutine( CheckIfWeCanDisplayNextSentence() );
	}

	IEnumerator PlayAudio()
	{
		if ( audioSource.clip )
		{
			//Debug.Log("Playing audio clip : " + audioSource.clip);
			audioSource.Play();
			while ( audioSource.isPlaying )
			{
				yield return null;
			}
		}

		isAudioPlayingFinished = true;
		StartCoroutine( CheckIfWeCanDisplayNextSentence() );
	}

	IEnumerator CheckIfWeCanDisplayNextSentence()
	{
		if ( isSentenceWritingFinished && isAudioPlayingFinished )
		{
			yield return new WaitForSeconds( timeBeetweenSentencesInterval );
			DisplayNextSentence();
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
}
