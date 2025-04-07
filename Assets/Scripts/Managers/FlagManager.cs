using System;
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
				if ( PlayerPrefs.GetInt( flag.FlagName, 0 ) != 0 )
				{
					PlayerPrefs.SetInt( flag.FlagName, PlayerPrefs.GetInt( flag.FlagName, 0 ) + 1 );
				}
				else
				{
					PlayerPrefs.SetInt( flag.FlagName, 1 );
				}

				PlayerPrefs.Save();
			}
			else
			{
				if ( temporaryFlags.ContainsKey( flag.FlagName ) )
				{
					temporaryFlags[flag.FlagName] += 100;
				}
				else
				{
					temporaryFlags[flag.FlagName] = 100;
				}
			}

			if (AnalyticsService.Instance != null)
			{
				FlagCompletedEvent flagEvent = new FlagCompletedEvent
				{
					FlagName = flag.FlagName
				};
				AnalyticsService.Instance.RecordEvent(flagEvent);
				Debug.Log($"Recorded event: flagCompleted with flagName = {flag.FlagName}");
			}
			else
			{
				Debug.LogError("AnalyticsService.Instance is null. Ensure Analytics is properly initialized.");
			}
		}
	}

	public bool IsFlagValid(Flag flag)
	{
		bool isActive = flag == null
			|| PlayerPrefs.GetInt( flag.FlagName, 0 ) >= 1
			|| temporaryFlags.ContainsKey( flag.FlagName );

		if ( isActive )
		{
			Debug.Log( $"Flag activated: {(flag != null ? flag.FlagName : null)}" );
		}

		return isActive;
	}

	public int GetFlagPoints(Flag flag)
	{
		if (flag == null)
		{
			Debug.Log("Flag is null, returning 0 points.");
			return 0;
		}

		int flagPoints = PlayerPrefs.GetInt(flag.FlagName, 0);
		if (temporaryFlags.ContainsKey(flag.FlagName))
		{
			flagPoints = Math.Max(flagPoints, temporaryFlags[flag.FlagName]);
		}

		Debug.Log( $"Flag {flag.FlagName} : " + flagPoints );

		return flagPoints;
	}

	public FlaggedScenarioNode GetBestValidFlaggedNode(List<FlaggedScenarioNode> flaggedNodes)
	{
		FlaggedScenarioNode bestNode = null;
		int maxPoints = -1;

		foreach (FlaggedScenarioNode flaggedNode in flaggedNodes)
		{
			if (IsFlagValid(flaggedNode.Flag))
			{
				int flagPoints = GetFlagPoints( flaggedNode.Flag );

				if (flagPoints > maxPoints)
				{
					maxPoints = flagPoints;
					bestNode = flaggedNode;
				}
			}
		}

		return bestNode;
	}
}
