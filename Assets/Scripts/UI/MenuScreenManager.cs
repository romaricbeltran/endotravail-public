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
            chapter.transform.Find("Image").GetComponent<Image>().color = Color.white;
            chapter.transform.Find("Lock").gameObject.SetActive(false);
            chapter.transform.Find("LockTitle").gameObject.SetActive(false);
            chapter.GetComponent<Animator>().SetBool("Unlocked", true);
        }
    }
}
