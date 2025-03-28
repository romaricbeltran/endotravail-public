using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeScreenManager : MonoBehaviour
{
	public Animator homeScreenTransition;
	public float creditTime = 4.5f;

	public void TriggerButton()
	{
		StartCoroutine(LoadCreditsAndNextScene());
	}

	public IEnumerator LoadCreditsAndNextScene()
	{
		homeScreenTransition.SetTrigger( "Start" );
		yield return new WaitForSeconds( creditTime );
		LevelLoader.LoadMenu();
	}
}
