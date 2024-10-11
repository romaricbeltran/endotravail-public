using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
	private Dictionary<string, int> temporaryFlags = new();

	public void SaveFlags(List<Flag> flags)
	{
		foreach ( Flag flag in flags )
		{
			if ( flag.PersistedInProgress )
			{
				PlayerPrefs.SetInt( flag.FlagName, 1 );
				PlayerPrefs.Save();
			}
			else
			{
				temporaryFlags[flag.FlagName] = 1;
			}

			AnalyticsService.Instance.CustomData( "flagCompleted", new Dictionary<string, object>
			{
				{ "flagName", flag.FlagName }
			} );
		}
	}

	public bool IsFlagActive(Flag flag)
	{
		if ( PlayerPrefs.GetInt( flag.FlagName, 0 ) == 1 || temporaryFlags.ContainsKey( flag.FlagName ) )
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
