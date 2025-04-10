using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
	private Dictionary<string, int> temporaryFlags = new();
	private Dictionary<string, int> persistedFlags = new();

	public void SaveFlags(List<Flag> flags)
	{
		foreach ( Flag flag in flags )
		{
			if ( flag.PersistedInProgress )
			{
				if (persistedFlags.ContainsKey(flag.FlagName))
				{
					persistedFlags[flag.FlagName]++;
				}
				else
				{
					int currentPlayerPrefs = PlayerPrefs.GetInt(flag.FlagName, 0);
					persistedFlags[flag.FlagName] = currentPlayerPrefs + 1;
				}
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
	
	public void CommitPersistedFlags()
	{
		foreach (var entry in persistedFlags)
		{
			int current = PlayerPrefs.GetInt(entry.Key, 0);
			PlayerPrefs.SetInt(entry.Key, current + entry.Value);
		}
		PlayerPrefs.Save();
		persistedFlags.Clear();
	}

	public bool IsFlagValid(Flag flag)
	{
		bool isActive = flag == null
			|| PlayerPrefs.GetInt( flag.FlagName, 0 ) >= 1
			|| persistedFlags.ContainsKey( flag.FlagName )
			|| temporaryFlags.ContainsKey( flag.FlagName );

		return isActive;
	}

	public int GetFlagPoints(Flag flag)
	{
		if (flag == null)
		{
			return 0;
		}

		int flagPoints = PlayerPrefs.GetInt(flag.FlagName, 0) + persistedFlags.GetValueOrDefault(flag.FlagName);
		
		if (temporaryFlags.ContainsKey(flag.FlagName))
		{
			flagPoints = Math.Max(flagPoints, temporaryFlags[flag.FlagName]);
		}
		
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

		if (bestNode != null)
		{
			Debug.Log($"Special flaggedNode activated: {bestNode.Flag?.FlagName}");
		}

		return bestNode;
	}
}
