using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueActionManager : BaseActionManager<DialogueAction>
{
	// UI
	public GameObject dialogueCanvas;
	public Image characterImage;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;
	public Button skipButton;

	public Animator dialogueBoxAnimator;
	public float typingLetterInterval = 0.03f;
	// public float autoProceedDelay = 1.5f; // Interval between sentences

	public AudioSource audioSource;

	private List<Sentence> sentences;
	private int indexSentence;
	private bool isTyping = false;
	private bool isAudioPlaying = false;
	private bool skippable = false;

	void Update()
	{
		// Allow skipping dialogue by pressing spacebar
		if ( skippable && Input.GetKeyDown( KeyCode.Space ) )
		{
			OnSkip();
		}
	}

	public override void LoadData(DialogueAction currentAction)
	{
		dialogueBoxAnimator.SetBool( "IsOpen", false );
		audioSource.Stop();
		StopAllCoroutines();

		indexSentence = 0;
		sentences = currentAction.Sentences;

		// Check for null or empty sentences
		if ( sentences == null || sentences.Count == 0 )
		{
			Debug.LogError( "No sentences to display!" );
			EndAction();
			return;
		}

		skipButton.onClick.AddListener( OnSkip );
	}

	public override void StartAction()
	{
		dialogueBoxAnimator.SetBool( "IsOpen", true );
		DisplayNextSentence();
	}

	public override void EndAction()
	{
		dialogueBoxAnimator.SetBool( "IsOpen", false );
		skipButton.onClick.RemoveAllListeners();
		audioSource.Stop();
		StopAllCoroutines();
		base.EndAction();
	}

	public void DisplayNextSentence()
	{
		audioSource.Stop();
		StopAllCoroutines();

		if ( indexSentence < sentences.Count )
		{
			var sentence = sentences[indexSentence];
			nameText.text = sentence.Character.CharacterName;
			characterImage.sprite = sentence.Character.CharacterImage;

			skippable = true;
			StartCoroutine( TypeSentence( sentence.Text ) );
			StartCoroutine( PlayAudio( sentence.AudioClip ) );

			indexSentence++;
		}
		else
		{
			skippable = false;
			EndAction();
		}
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		isTyping = true;

		foreach ( char letter in sentence )
		{
			if ( !isTyping ) break;
			dialogueText.text += letter;
			yield return new WaitForSeconds( typingLetterInterval );
		}

		dialogueText.text = sentence;
		isTyping = false;

		// Disable auto-proceed
		//CheckIfWeCanProceed();
	}

	IEnumerator PlayAudio(AudioClip clip)
	{
		if ( clip == null )
		{
			isAudioPlaying = false;
			yield break;
		}

		isAudioPlaying = true;
		audioSource.clip = clip;
		audioSource.Play();
		yield return new WaitWhile( () => audioSource.isPlaying );

		isAudioPlaying = false;

		// Disable auto-proceed
		//CheckIfWeCanProceed();
	}

	public void OnSkip()
	{
		// If typing is in progress, stop and show the full sentence
		if ( isTyping )
		{
			StopCoroutine( "TypeSentence" );
			dialogueText.text = sentences[indexSentence - 1].Text;
			isTyping = false;
		}
		// If not, move to the next sentence
		else
		{
			DisplayNextSentence();
		}
	}

	// Disable auto-proceed
	//private void CheckIfWeCanProceed()
	//{
	//	if ( !isTyping && !isAudioPlaying )
	//	{
	//		StartCoroutine( WaitBeforeProceeding() );
	//	}
	//}

	//IEnumerator WaitBeforeProceeding()
	//{
	//	yield return new WaitForSeconds( autoProceedDelay );
	//	DisplayNextSentence();
	//}
}
