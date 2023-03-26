using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using Unity.Services.Core.Environments;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private void Awake() 
    { 
        if (Instance != null) 
        { 
            Destroy(gameObject); 
            return; 
        }

        Instance = this; 
        DontDestroyOnLoad(gameObject); 
    }

    async void Start()
    {
        try
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("development");
            await UnityServices.InitializeAsync(options);
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            Debug.Log("StartAnalytics");
        }
        catch (ConsentCheckException e)
        {
            Debug.Log("Fail to Initialize Analytics");
          // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }
    }

    public void SendCustomEvent(string eventName, string parameterName, object parameterValue)
    {
        AnalyticsService.Instance.CustomData(eventName, new Dictionary<string, object>
        {
            { parameterName, parameterValue }
        });
    }
}