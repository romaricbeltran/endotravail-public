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
					temporaryFlags[flag.FlagName] += 1;
				}
				else
				{
					temporaryFlags[flag.FlagName] = 1;
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

	public FlaggedScenarioNode GetBestValidFlaggedNode(List<FlaggedScenarioNode> flaggedNodes)
	{
		FlaggedScenarioNode bestNode = null;
		int maxPoints = -1;

		foreach (FlaggedScenarioNode flaggedNode in flaggedNodes)
		{
			if (IsFlagValid(flaggedNode.Flag))
			{
				int flagPoints = 0;

				if ( flaggedNode.Flag != null )
				{
					flagPoints = PlayerPrefs.GetInt(flaggedNode.Flag.FlagName, 0);
					Debug.Log( $"Flag points: {flagPoints}" );

					if (temporaryFlags.ContainsKey(flaggedNode.Flag.FlagName))
					{
						flagPoints = Math.Max(flagPoints, temporaryFlags[flaggedNode.Flag.FlagName]);
					}
				}

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
