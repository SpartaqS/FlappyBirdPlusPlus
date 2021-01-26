using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

using System.Linq;

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

        public int birdColor;

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
                    Debug.Log("Default");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log("Cached");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log("Remote - loading");
                    birdColor = 0;
                    birdColor = ConfigManager.appConfig.GetInt("birdColor");

                    float red = (birdColor / (Mathf.Pow(256f,2)))/ 255f;
                    float green = ((birdColor / 256) % 256) / 255f;
                    float blue = (birdColor % 256)/ 255f;

                    Color loadedBirdColor = new Color(red , green, blue, 1f);

                    gameSettings.ApplyRemoteConfig(loadedBirdColor);
                    break;                    
            }            
        }

        private void OnDestroy()
        {
            ConfigManager.FetchCompleted -= ApplyRemoteSettings;
        }
    }
}