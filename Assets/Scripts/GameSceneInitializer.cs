using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    public class GameSceneInitializer : MonoBehaviour
    {
        [SerializeField] GameSettings gameSettings;
        [SerializeField] Transform startingPosition;
        // Start is called before the first frame update
        void Start()
        {
            GameObject playerGameObject = Instantiate(gameSettings.PlayerPrefab, startingPosition.position, startingPosition.rotation);
            Destroy(startingPosition.gameObject);
            // Initialize the GameplayManager (I guess just call "initialize") - the gameplay manager will "unpause" the bird when it is done doing stuff
            // pass the playerGameObject to the GameplayManager so that it can have it
        }
    }
}