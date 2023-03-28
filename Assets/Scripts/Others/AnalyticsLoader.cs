using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using Unity.Services.Core.Environments;

public class AnalyticsLoader : MonoBehaviour
{
    async void Start()
    {
        try
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options);
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            //Debug.Log("StartAnalytics");
        }
        catch (ConsentCheckException e)
        {
            Debug.Log("Fail to Initialize Analytics");
          // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }
    }
}