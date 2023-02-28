using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private const string PROGRESS_KEY = "player_progress";

    public void SaveProgress(int levelNumber)
    {
        PlayerPrefs.SetInt(PROGRESS_KEY, levelNumber);
        PlayerPrefs.Save();
    }

    public int LoadProgress()
    {
        return PlayerPrefs.GetInt(PROGRESS_KEY, 0);
    }
}
