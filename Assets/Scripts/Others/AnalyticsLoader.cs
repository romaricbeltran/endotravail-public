using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using Unity.Services.Core.Environments;

public class AnalyticsLoader : MonoBehaviour
{
    public static AnalyticsLoader instance;

    void Awake()
    {
        // Vérifie si une autre instance de existe déjà, dans ce cas détruit cet objet pour s'assurer qu'il n'y a qu'une seule instance
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
            options.SetEnvironmentName("production");
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