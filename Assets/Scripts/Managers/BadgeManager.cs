using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;

public class BadgeManager : MonoBehaviour
{
	// UI
	public GameObject badgeCanvas;
	public TextMeshProUGUI mainText;

	public void LoadData(Badge currentBadge)
	{
		mainText.text = currentBadge.BadgeName;

		AnalyticsService.Instance.CustomData( "badgeCompleted", new Dictionary<string, object>
		{
			{ "badgeName", currentBadge.BadgeName }
		} );
	}

	public void StartNode()
	{
		badgeCanvas.SetActive( true );
	}

	public void EndNode()
	{
		badgeCanvas.SetActive( false );
	}
}
