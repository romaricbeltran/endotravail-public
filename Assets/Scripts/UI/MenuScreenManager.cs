using UnityEngine;
using UnityEngine.UI;

public class MenuScreenManager : MonoBehaviour
{
	public Button [] chapterButtons;

	public int progress;

	private void Start()
	{
		progress = GameManager.LoadProgress();

		EnableChapters();
	}

	public void EnableChapters()
    {
		for (int i = 0; i <= progress && i < chapterButtons.Length; i++)
        {
			chapterButtons[i].transform.Find("Image").GetComponent<Image>().color = Color.white;
			chapterButtons[i].transform.Find("Lock").gameObject.SetActive(false);
			chapterButtons[i].GetComponent<Animator>().SetBool( "Unlocked", true );
		}
    }

	public void LoadLevelButton(int level)
	{
		// Pass Home & Menu scene index
		if ( level - 2 <= progress)
		{
			LevelLoader.instance.LoadLevel( level );
			GetComponent<GraphicRaycaster>().enabled = false;
		}
	}
}
