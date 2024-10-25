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

		AnalyticsService.Instance.CustomData( "badgeCompleted", new Dictionary<string, object>
		{
			{ "badgeName", badge.BadgeName }
		} );
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
