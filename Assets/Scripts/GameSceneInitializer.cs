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
        // Start is called before the first frame update
        void Start()
        {
            GameObject playerGameObject = Instantiate(gameSettings.PlayerPrefab, startingPosition.position, startingPosition.rotation);            

            List<Pipe> allPipes = Resources.LoadAll<Pipe>("Pipes").ToList(); // load all pipes

            GetComponent<GameplayManager>().Initialize(playerGameObject, allPipes, new ObjectPool(pipePrefab, 3, transform), startingPosition.transform.position.x);
            // Initialize the GameplayManager (I guess just call "initialize") - the gameplay manager will "unpause" the bird when it is done doing stuff
            // pass the playerGameObject to the GameplayManager so that it can have it
            Destroy(startingPosition.gameObject);
        }
    }
}