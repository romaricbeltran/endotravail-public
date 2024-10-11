using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using Unity.Services.Core.Environments;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }

    async void Start()
    {
        try
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("development");
            await UnityServices.InitializeAsync(options);
            AnalyticsService.Instance.StartDataCollection();
            //Debug.Log("StartAnalytics");
        }
        catch (ConsentCheckException e)
        {
            Debug.Log("Fail to Initialize Analytics");
          // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }
    }
}
