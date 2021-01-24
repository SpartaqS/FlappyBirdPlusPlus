﻿using System.Collections;
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
        [SerializeField] RectTransform hintImage = null;

        [SerializeField] GameOverDisplay gameOverDisplay = null;

        // Start is called before the first frame update
        void Start()
        {
            GameObject playerGameObject = Instantiate(gameSettings.PlayerPrefab, (startingPosition.position + Vector3.left * 100f), startingPosition.rotation);
            BirdController playerController = playerGameObject.GetComponent<BirdController>();        

            List<Pipe> allPipes = Resources.LoadAll<Pipe>("Pipes").ToList(); // load all pipes
            GameplayManager gameplayManager = GetComponent<GameplayManager>();

            HintImage hintImage = FindObjectOfType<HintImage>();
            LeanTween.alpha(hintImage.GetComponent<RectTransform>(), 1f, 1f).setDelay(3f);

            playerController.Initialize(gameSettings, startingPosition.transform.position, gameplayManager.StartGame, hintImage.Hide);
            gameplayManager.Initialize(playerController, allPipes, new ObjectPool(pipePrefab, 3, transform), startingPosition.transform.position.x, gameSettings);
            // Initialize the GameplayManager (I guess just call "initialize") - the gameplay manager will "unpause" the bird when it is done doing stuff
            // pass the playerGameObject to the GameplayManager so that it can have it
            Destroy(startingPosition.gameObject);

            ExplosionManager explosionManager = GetComponent<ExplosionManager>();
            explosionManager.Initialize(gameplayManager, playerController, new ObjectPool(explosionPrefab,1,transform), gameSettings);

            // hook up UI
            FindObjectOfType<BombDisplay>().Initialize(gameSettings, gameplayManager);
            FindObjectOfType<ScoreDisplay>().Initialize(gameplayManager);

            gameOverDisplay.Initialize(gameplayManager);
            gameOverDisplay.gameObject.SetActive(false);

            Destroy(this); // no need to keep the initializer around
        }
    }
}