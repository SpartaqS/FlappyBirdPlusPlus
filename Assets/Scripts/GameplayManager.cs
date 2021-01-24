using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    public class GameplayManager : MonoBehaviour
    {
        public System.Action<int, int> updateBombProgress;
        public System.Action<int> updateScore;
        public System.Action useBomb;
        public System.Action<int> announceEndGame;

        List<Pipe> pipeTypes = new List<Pipe>(); // all pipe types (created using the Pipe ScriptableObject)
        BirdController playerController = null;

        ObjectPool pipeObjectPool = null; // handles inactive pipes (re-use existing pipes or create/destroy more on-demand)
        List<ActivePipe> activePipes = new List<ActivePipe>(); // to handle active pipes

        /* "Bird" parameters */
        [SerializeField] int score;
        float speed;
        float birdPositionX;

        /* Pipe spawning parameters */
        bool keepSpawningPipes = false;
        const float PIPE_WIDTH = 26f;
        const float PIPE_HEIGHT = 160f;

        const float DISPOSE_PIPE_POSITION_X = -100f;
        const float SPAWN_PIPE_POSITION_X = 100f;
        float latestY;
        float gapSize;
        // those two need to be recalculated on each change of speed/gap size to keep the game beatable
        float maxYAscend;
        float maxYDrop;

        float timer = 0f;
        float timerMax; // how long should we wait for a new pipe to be spawned        

        /* Bomb related fields */
        [SerializeField] int bombCount;
        [SerializeField] int bombProgress;
        int scoreForExtraBomb = 2;
        int maxBombCount = 3;
        const float EXPLOSION_MAGNITUDE = 10000f;

        public void Initialize(BirdController playerController, List<Pipe> pipeTypes, ObjectPool pipeObjectPool, float birdPositionX, GameSettings gameSettings)
        {
            this.playerController = playerController;

            playerController.tryUseBomb += HandleBombRequest;
            playerController.onDeath += HandleDeath;

            this.pipeTypes = pipeTypes;       
            this.pipeObjectPool = pipeObjectPool;
            this.birdPositionX = birdPositionX;

            Debug.Log("Loaded " + pipeTypes.Count + " distinct pipe types");

            speed = gameSettings.Speed;
            timerMax = gameSettings.PipeSpawnTimer;

            scoreForExtraBomb = gameSettings.ScoreForExtraBomb;
            maxBombCount = gameSettings.MaxBombCount;

            score = 0;
            latestY = 0f;
            gapSize = gameSettings.PipeGapSize;
        }

        public void StartGame()
        {
            keepSpawningPipes = true;
            timer = 0f;
            CreatePipe(SPAWN_PIPE_POSITION_X, latestY, gapSize);
        }

        private void Update()
        {
            MovePipes();

            timer += Time.deltaTime;
            if(timer >= timerMax)
            {
                timer -= timerMax;
                if (keepSpawningPipes)
                {
                    CreatePipe(SPAWN_PIPE_POSITION_X, latestY, gapSize);
                }
            }
        }


        #region PipeCreation

        private void CreatePipe(float x, float y, float gapSize)
        {
            Pipe pipeToCreate = ObtainPipe();

            GameObject newPipe = pipeObjectPool.TakeObjectFromPool();

            newPipe.transform.position = new Vector3(x, y, 0f);

            Transform bottomPipe = newPipe.transform.Find("PipeBottom");

            SpriteRenderer currentSprite = bottomPipe.GetComponent<SpriteRenderer>();
            currentSprite.sprite = pipeToCreate.bottomPipe.PipeSprite;
            currentSprite.color = pipeToCreate.bottomPipe.PipeColor;
            
            Transform topPipe = newPipe.transform.Find("PipeTop");
            currentSprite = topPipe.GetComponent<SpriteRenderer>();
            currentSprite.sprite = pipeToCreate.topPipe.PipeSprite;
            currentSprite.color = pipeToCreate.topPipe.PipeColor;

            topPipe.transform.localPosition = new Vector3(0f, PIPE_HEIGHT/2 + gapSize / 2, 0f);
            /*topPipe.transform.rotation = Quaternion.identity;
            topPipe.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;*/
            bottomPipe.transform.localPosition = new Vector3(0f, -PIPE_HEIGHT/2 - gapSize / 2, 0f);

            activePipes.Add(new ActivePipe(newPipe.transform));
        }

        private Pipe ObtainPipe()
        {            
            List<Pipe> avaliablePipes = new List<Pipe>();
            foreach(Pipe pipe in pipeTypes)
            {
                if(pipe.MinimumScore <= score && (!pipe.RestrictMaximumScore || pipe.MaximumScore >= score))
                {
                    avaliablePipes.Add(pipe);         
                }                
            }

            int selectedPipeIndex = Random.Range(0, avaliablePipes.Count);
                        
            return avaliablePipes[selectedPipeIndex];
        }

        private void CalculateMaxDropAndAscend()
        {

        }

        #endregion

        #region Pipe Movement and Logic
        private void MovePipes()
        {
            for (int i = 0; i < activePipes.Count; i++)
            {
                ActivePipe currentPipe = activePipes[i];
                currentPipe.representedPipe.position += Vector3.left * speed * Time.deltaTime;
                
                if(!currentPipe.hasBeenPassed && currentPipe.representedPipe.position.x <= birdPositionX)
                {
                    if (!playerController.Renderer.isVisible) // player is off-screen so they should have hit the pipe
                    {                        
                        playerController.TryToDie();
                        playerController.transform.position = new Vector3(currentPipe.representedPipe.position.x - PIPE_WIDTH * 0.75f, playerController.transform.position.y, playerController.transform.position.z); // move the bird to "simulate" being hit by the pipe collider
                    }
                    else
                    {
                        ++score;
                        TryObtainBomb();
                        updateScore?.Invoke(score);
                    }
                    currentPipe.hasBeenPassed = true;
                }

                if (currentPipe.representedPipe.position.x <= DISPOSE_PIPE_POSITION_X)
                {
                    activePipes.RemoveAt(i);
                    pipeObjectPool.ReturnObjectToPool(currentPipe.representedPipe.gameObject);
                    --i;
                }
            }
        }

        private void TryObtainBomb()
        {
            if (bombCount < maxBombCount)
            {
                ++bombProgress;
                if (bombProgress >= scoreForExtraBomb)
                {
                    bombProgress -= scoreForExtraBomb;
                    ++bombCount;
                }
                updateBombProgress?.Invoke(bombProgress, bombCount);
            }
        }
        #endregion

        #region Bomb Handling
        private void HandleBombRequest()
        {
            if(bombCount > 0)
            {
                --bombCount;
                updateBombProgress?.Invoke(bombProgress, bombCount);
                useBomb?.Invoke();

                foreach(ActivePipe pipe in activePipes)
                {
                    pipe.hasBeenPassed = true; // score only for passing pipes
                    pipe.representedPipe.gameObject.SetActive(false);
                    /*Rigidbody2D topPipe = pipe.representedPipe.Find("PipeTop").GetComponent<Rigidbody2D>();
                    topPipe.bodyType = RigidbodyType2D.Dynamic;

                    Vector2 DifferenceVector = topPipe.position - new Vector2(playerController.transform.position.x, playerController.transform.position.y);

                    topPipe.AddForce(DifferenceVector * EXPLOSION_MAGNITUDE / Mathf.Pow(DifferenceVector.magnitude, 1), ForceMode2D.Force);*/
                }
            }
        }

        #endregion

        #region End Game Handling

        private void HandleDeath()
        {
            keepSpawningPipes = false;
            speed = 0f; // stop the bird from "moving"
            announceEndGame?.Invoke(score);
        }

        #endregion

        private class ActivePipe
        {
            public Transform representedPipe;
            public bool hasBeenPassed = false;

            public ActivePipe(Transform pipeTransform)
            {
                representedPipe = pipeTransform;
                hasBeenPassed = false;
            }
        }
    }
}