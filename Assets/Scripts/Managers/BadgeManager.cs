using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;

public class BadgeManager : MonoBehaviour
{
	// UI
	public GameObject badgeCanvas;
	public TextMeshProUGUI mainText;

	public void LoadBadge(Badge badge)
	{
		mainText.text = badge.BadgeName;
		StartBadge();

		if (AnalyticsService.Instance != null)
		{
			BadgeCompletedEvent badgeEvent = new BadgeCompletedEvent
			{
				BadgeName = badge.BadgeName
			};
			AnalyticsService.Instance.RecordEvent(badgeEvent);
			Debug.Log($"Recorded event: badgeCompleted with badgeName = {badge.BadgeName}");
		}
		else
		{
			Debug.LogError("AnalyticsService.Instance is null. Ensure Analytics is properly initialized.");
		}
	}

	public void StartBadge()
	{
		badgeCanvas.SetActive( true );
	}

	public void EndBadge()
	{
		badgeCanvas.SetActive( false );
	}
}
