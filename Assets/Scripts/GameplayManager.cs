using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    public class GameplayManager : MonoBehaviour
    {
        List<Pipe> pipeTypes = new List<Pipe>(); // all pipe types (created using the Pipe ScriptableObject)
        GameObject playerObject = null;

        ObjectPool pipeObjectPool = null; // handles inactive pipes (re-use existing pipes or create/destroy more on-demand)
        List<ActivePipe> activePipes = new List<ActivePipe>(); // to handle active pipes

        /* "Bird" parameters */
        [SerializeField] int score;
        float speed;
        float birdPositionX;

        /* Pipe spawning parameters */
        const float PIPE_HEIGHT = 160f;

        const float DISPOSE_PIPE_POSITION_X = -100f;
        const float SPAWN_PIPE_POSITION_X = 100f;
        float latestY;
        float gapSize;

        float timer = 0f;
        float timerMax; // how long should we wait for a new pipe to be spawned        

        /* Bomb related fields */
        [SerializeField] int bombCount;
        [SerializeField] int bombProgress;
        int scoreForExtraBomb = 2;
        int maxBombCount = 3;       

        public void Initialize(GameObject playerObject, List<Pipe> pipeTypes, ObjectPool pipeObjectPool, float birdPositionX, GameSettings gameSettings)
        {
            this.playerObject = playerObject;
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

            CreatePipe(SPAWN_PIPE_POSITION_X, latestY, gapSize);
        }

        private void Update()
        {
            MovePipes();

            timer += Time.deltaTime;
            if(timer >= timerMax)
            {
                timer -= timerMax;
                CreatePipe(SPAWN_PIPE_POSITION_X, latestY, gapSize);
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
                    currentPipe.hasBeenPassed = true;
                    ++score;
                    TryObtainBomb();
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
            if(bombCount < maxBombCount)
            {
                ++bombProgress;
                if(bombProgress >= scoreForExtraBomb)
                {
                    bombProgress -= scoreForExtraBomb;
                    ++bombCount;
                }
            }
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