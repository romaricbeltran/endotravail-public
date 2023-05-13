using UnityEngine;
using UnityEngine.UI;

public class MenuScreenManager : MonoBehaviour
{
    public LevelLoader levelLoader;

    public static void EnableChapters()
    {
        int progress = PlayerPrefs.GetInt(LevelLoader.PLAYER_PROGRESS, 0);

        for (int i = 1; i <= progress && i < 5; i++)
        {
            GameObject chapter = GameObject.Find("Chapter" + i);
            Image chapterImage = chapter.GetComponent<Image>();
            chapterImage.color = Color.white;
            chapter.transform.Find("Lock").gameObject.SetActive(false);
        }
    }
}
