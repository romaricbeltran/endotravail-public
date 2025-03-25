using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
	public class NarrativeScreenManager : MonoBehaviour
	{
		public TextMeshProUGUI narrativeText;
		public Animator screenAnimator;
		public float typingLetterInterval = 0.03f;
		public float delayBeforeTyping = 3f;
		public float delayBetweenSentences = 1.5f;
		public string[] sentences;

		private bool isTyping = false;

		public void StartNarrative()
		{
			StartCoroutine(PlayNarrativeSequence());
		}

		private IEnumerator PlayNarrativeSequence()
		{
			screenAnimator.SetBool("IsOpen", true);

			yield return new WaitForSeconds(delayBeforeTyping);

			for (int i = 0; i < sentences.Length; i++)
			{
				if (i > 0)
				{
					narrativeText.text += "\n\n";
				}

				yield return StartCoroutine(TypeText(sentences[i]));
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
