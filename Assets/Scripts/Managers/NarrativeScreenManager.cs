using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
	[System.Serializable]
	public class NarrativeSequence
	{
		[TextArea(3, 10)]
		public string[] sentences;
	}
	
	public class NarrativeScreenManager : MonoBehaviour
	{
		public TextMeshProUGUI narrativeText;
		public Animator screenAnimator;
		public float typingLetterInterval = 0.03f;
		public float delayBeforeTyping = 3f;
		public float delayBetweenSentences = 1.5f;
		
		public NarrativeSequence[] sequences;

		private int currentSequenceIndex = 0;
		private bool isTyping = false;

		public void StartNarrative()
		{
			if (currentSequenceIndex >= sequences.Length)
			{
				Debug.Log("Toutes les séquences ont été jouées.");
				return;
			}

			StartCoroutine(PlayNarrativeSequence(sequences[currentSequenceIndex]));
			currentSequenceIndex++;
		}

		private IEnumerator PlayNarrativeSequence(NarrativeSequence sequence)
		{
			screenAnimator.SetBool("IsOpen", true);
			narrativeText.text = "";

			yield return new WaitForSeconds(delayBeforeTyping);

			for (int i = 0; i < sequence.sentences.Length; i++)
			{
				if (i > 0)
				{
					narrativeText.text += "\n\n";
				}

				yield return StartCoroutine(TypeText(sequence.sentences[i]));
				yield return new WaitForSeconds(delayBetweenSentences);
			}

			screenAnimator.SetBool("IsOpen", false);
		}

		private IEnumerator TypeText(string text)
		{
			isTyping = true;

			foreach (char letter in text)
			{
				if (!isTyping) break;
				narrativeText.text += letter;
				yield return new WaitForSeconds(typingLetterInterval);
			}

			isTyping = false;
		}
	}
}
