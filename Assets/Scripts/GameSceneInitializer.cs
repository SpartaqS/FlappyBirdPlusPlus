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

        [SerializeField] GameOverDisplay gameOverDisplay = null;

        // Start is called before the first frame update
        void Awake()
        {
            Initialize();            
        }

        private void Initialize()
        {           
            Camera.main.orthographicSize = GameSceneConstants.CAMERA_SIZE; //Needs to be adjusted because a lot of game logic relies on the height of the visible world //I do this call to Camera.main only once so that's not a big performance hit.

            GameObject playerGameObject = Instantiate(gameSettings.PlayerPrefab, (startingPosition.position + Vector3.left * 100f), startingPosition.rotation);
            BirdController playerController = playerGameObject.GetComponent<BirdController>();

            List<Pipe> allPipes = Resources.LoadAll<Pipe>("Pipes").ToList(); // load all pipes
            GameplayManager gameplayManager = GetComponent<GameplayManager>();

            HintImage hintImage = FindObjectOfType<HintImage>();
            LeanTween.alpha(hintImage.GetComponent<RectTransform>(), 1f, 1f).setDelay(3f);
            HumanInputWrapper inputWrapper = new HumanInputWrapper();

            playerController.Initialize(inputWrapper, gameSettings, startingPosition.transform.position, gameplayManager.StartGame, hintImage.Hide);
            gameplayManager.Initialize(playerController, allPipes, new ObjectPool(pipePrefab, 3, transform), startingPosition.transform.position.x, gameSettings);
            // Initialize the GameplayManager (I guess just call "initialize") - the gameplay manager will "unpause" the bird when it is done doing stuff
            // pass the playerGameObject to the GameplayManager so that it can have it
            Destroy(startingPosition.gameObject);

            ExplosionManager explosionManager = GetComponent<ExplosionManager>();
            explosionManager.Initialize(gameplayManager, playerController, new ObjectPool(explosionPrefab, 1, transform), gameSettings);

            // hook up UI
            FindObjectOfType<BombDisplay>().Initialize(gameSettings, gameplayManager);
            FindObjectOfType<ScoreDisplay>().Initialize(gameplayManager);

            gameOverDisplay.Initialize(gameplayManager);
            gameOverDisplay.gameObject.SetActive(false);
            Destroy(this); // no need to keep the initializer around
        }
    }
}