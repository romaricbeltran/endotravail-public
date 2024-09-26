using UnityEngine;
using UnityEngine.UI;

public class MenuScreenManager : MonoBehaviour
{
	public Button [] chapterButtons;

	public int progress;

	private void Start()
	{
		progress = LevelLoader.LoadProgress();

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
		if ( level <= progress )
		{
			LevelLoader.instance.LoadLevel( level );
			GetComponent<GraphicRaycaster>().enabled = false;
		}
	}
}
