using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

using UnityEngine.Analytics;
//using System.Linq;

namespace FlappyBirdPlusPlus
{
    public class RemoteConfig : MonoBehaviour
    {
        [SerializeField]
        GameSettings gameSettings = null;

        public struct userAttributes
        {

        }

        public struct appAtributes
        {
            
        }

        public string birdColor;

        private void Awake()
        {
            ConfigManager.FetchCompleted += ApplyRemoteSettings;

            ConfigManager.FetchConfigs<userAttributes, appAtributes>(new userAttributes(), new appAtributes());

        }

        private void ApplyRemoteSettings(ConfigResponse configResponse)        
        {
            switch (configResponse.requestOrigin)
            {
                case ConfigOrigin.Default:
                    break;
                case ConfigOrigin.Cached:
                    break;
                case ConfigOrigin.Remote:
                    birdColor = ConfigManager.appConfig.GetString("BirdColor");

                    Color loadedBirdColor;
                    if (ColorUtility.TryParseHtmlString(birdColor, out loadedBirdColor))
                    {
                        if (AnalyticsSessionInfo.sessionElapsedTime < 10000f)
                        {
                            AnalyticsResult analyticsResult = Analytics.CustomEvent("Bird Color", new Dictionary<string, object>{ { "Hex Value:", birdColor } });
                        }
                        gameSettings.ApplyRemoteConfig(loadedBirdColor);
                    }
                    else // there was an error when loading the color (fall back to the untinted bird variant)
                    {
                        if (AnalyticsSessionInfo.sessionElapsedTime < 10000f)
                        {
                            loadedBirdColor = new Color(1f, 1f, 1f, 1f);
                            AnalyticsResult analyticsResult = Analytics.CustomEvent("Bird Color", new Dictionary<string, object> { { "Not Obtained (default)", birdColor } });
                        }                        
                        gameSettings.ApplyRemoteConfig(loadedBirdColor);
                    }
                    break;                    
            }            
        }

        private void OnDestroy()
        {
            ConfigManager.FetchCompleted -= ApplyRemoteSettings;
        }
    }
}