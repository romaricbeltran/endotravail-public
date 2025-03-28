using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;
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
	    await InitializeAnalytics();
    }

    private async Task InitializeAnalytics()
    {
	    try
	    {
		    var options = new InitializationOptions();
		    options.SetEnvironmentName("development");
		    await UnityServices.InitializeAsync(options);

		    if (AnalyticsService.Instance != null)
		    {
			    AnalyticsService.Instance.StartDataCollection();
			    Debug.Log("Analytics initialized and data collection started.");
		    }
	    }
	    catch (System.Exception e)
	    {
		    Debug.LogError($"Unexpected error initializing Analytics: {e.Message}");
	    }
    }
}
