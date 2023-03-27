using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;

public class BadgeManager : MonoBehaviour
{
    //UI
    public GameObject badgeCanvas;
    public TextMeshProUGUI mainText;

    public List<string> badges;

    public void LoadBadge(int badgeIndex)
    {
        mainText.text = badges[badgeIndex];
        AnalyticsService.Instance.CustomData("badgeCompleted", new Dictionary<string, object>
        {
            { "badgeName", badges[badgeIndex] }
        });
    }

    public void StartBadge()
    {
        badgeCanvas.SetActive(true);
    }

    public void EndBadge()
    {
        badgeCanvas.SetActive(false);
    }
}
