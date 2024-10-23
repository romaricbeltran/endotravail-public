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
	public float typingLetterInterval = 0.05f;
	public float autoProceedDelay = 1.5f; // Temps de pause avant de passer automatiquement à la prochaine phrase

	public AudioSource audioSource;

	private List<Sentence> sentences;
	private int indexSentence;
	private bool isTyping = false;
	private bool isAudioPlaying = false;

	// On précharge le dialogue avec la première phrase
	public override void LoadData(DialogueAction currentAction)
	{
		dialogueBoxAnimator.SetBool( "IsOpen", false );
		audioSource.Stop();
		StopAllCoroutines();

		indexSentence = 0;
		sentences = currentAction.Sentences;

		skipButton.onClick.RemoveAllListeners();
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
		audioSource.Stop();  // On s'assure que l'audio s'arrête
		StopAllCoroutines(); // Stop toutes les coroutines en cours
		base.EndAction();
	}

	public void DisplayNextSentence()
	{
		StopAllCoroutines(); // On stoppe les coroutines en cours
		audioSource.Stop();   // Arrête l'audio précédent si en cours

		if ( indexSentence < sentences.Count )
		{
			var sentence = sentences[indexSentence];
			nameText.text = sentence.Character.CharacterName;
			characterImage.sprite = sentence.Character.CharacterImage;

			StartCoroutine( TypeSentence( sentence.Text ) );
			StartCoroutine( PlayAudio( sentence.AudioClip ) );

			indexSentence++;
		}
		else
		{
			EndAction(); // On termine si on est à la dernière phrase
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

		dialogueText.text = sentence; // On affiche le texte en entier si skip
		isTyping = false;

		CheckIfWeCanProceed(); // Vérifie si on peut avancer après le texte
	}

	IEnumerator PlayAudio(AudioClip clip)
	{
		if ( clip )
		{
			isAudioPlaying = true;
			audioSource.clip = clip;
			audioSource.Play();
			yield return new WaitWhile( () => audioSource.isPlaying );
		}

		isAudioPlaying = false;
		CheckIfWeCanProceed(); // Vérifie si on peut avancer après l'audio
	}

	public void OnSkip()
	{
		if ( isTyping )
		{
			// On complète immédiatement le texte en cours de tapage
			isTyping = false;
			StopCoroutine( "TypeSentence" );
			dialogueText.text = sentences[indexSentence - 1].Text;
		}

		if ( isAudioPlaying )
		{
			// On arrête l'audio immédiatement
			audioSource.Stop();
			isAudioPlaying = false;
		}

		// Passe directement à la phrase suivante ou termine si c'est la dernière
		DisplayNextSentence();
	}

	private void CheckIfWeCanProceed()
	{
		// Si l'audio et le texte sont terminés, on passe automatiquement à la phrase suivante après un délai
		if ( !isTyping && !isAudioPlaying )
		{
			StartCoroutine( WaitBeforeProceeding() );
		}
	}

	IEnumerator WaitBeforeProceeding()
	{
		// Ajoute une pause avant de passer à la phrase suivante
		yield return new WaitForSeconds( autoProceedDelay );
		DisplayNextSentence();
	}
}
