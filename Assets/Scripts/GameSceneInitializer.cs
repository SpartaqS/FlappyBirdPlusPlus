using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace FlappyBirdPlusPlus
{
    public class GameSceneInitializer : MonoBehaviour
    {
        [SerializeField] GameSettings gameSettings = null;
        [SerializeField] Transform startingPosition= null;
        [SerializeField] GameObject pipePrefab = null;
        [SerializeField] GameObject explosionPrefab = null;
        // Start is called before the first frame update
        void Start()
        {
            GameObject playerGameObject = Instantiate(gameSettings.PlayerPrefab, startingPosition.position, startingPosition.rotation);
            BirdController playerController = playerGameObject.GetComponent<BirdController>();
            playerGameObject.GetComponent<Rigidbody2D>().gravityScale = gameSettings.GravityScale;

            List<Pipe> allPipes = Resources.LoadAll<Pipe>("Pipes").ToList(); // load all pipes

            GameplayManager gameplayManager = GetComponent<GameplayManager>();
            gameplayManager.Initialize(playerController, allPipes, new ObjectPool(pipePrefab, 3, transform), startingPosition.transform.position.x, gameSettings);
            // Initialize the GameplayManager (I guess just call "initialize") - the gameplay manager will "unpause" the bird when it is done doing stuff
            // pass the playerGameObject to the GameplayManager so that it can have it
            Destroy(startingPosition.gameObject);

            ExplosionManager explosionManager = GetComponent<ExplosionManager>();
            explosionManager.Initialize(gameplayManager, playerController, explosionPrefab);

            // hook up UI
            FindObjectOfType<BombDisplay>().Initialize(gameSettings, gameplayManager);

            Destroy(this); // no need to keep the initializer around
        }
    }
}