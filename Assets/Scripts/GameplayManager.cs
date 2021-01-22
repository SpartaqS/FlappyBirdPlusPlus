using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    public class GameplayManager : MonoBehaviour
    {
        List<Pipe> pipeTypes = new List<Pipe>();
        GameObject playerObject = null;        

        ObjectPool pipeObjectPool = null;
        List<ActivePipe> activePipes = new List<ActivePipe>();
        [SerializeField] int score;
        const float DISPOSE_PIPE_POSITION_X = -100f;
        const float SPAWN_PIPE_POSITION_X = 100f;
        float birdPositionX;

        float speed = 30f;

        float timer = 0f;
        float timerMax = 3f; // how long should we wait for a new pipe to be spawned

        public void Initialize(GameObject playerObject, List<Pipe> pipeTypes, ObjectPool pipeObjectPool, float birdPositionX)
        {
            this.playerObject = playerObject;
            this.pipeTypes = pipeTypes;            
            this.pipeObjectPool = pipeObjectPool;
            this.birdPositionX = birdPositionX;

            Debug.Log("Loaded " + pipeTypes.Count + " distinct pipe types");

            score = 0;

            CreatePipe(SPAWN_PIPE_POSITION_X);
        }

        private void Update()
        {
            MovePipes();

            timer += Time.deltaTime;
            if(timer >= timerMax)
            {
                timer -= timerMax;
                CreatePipe(SPAWN_PIPE_POSITION_X);
            }
        }


        #region PipeCreation

        private void CreatePipe(float x)
        {
            Pipe pipeToCreate = ObtainPipe();

            GameObject newPipe = pipeObjectPool.TakeObjectFromPool();

            newPipe.transform.position = new Vector3(x, 0, 0);

            Transform bottomPipe = newPipe.transform.Find("PipeBottom");
            SpriteRenderer currentSprite = bottomPipe.GetComponent<SpriteRenderer>();
            currentSprite.sprite = pipeToCreate.bottomPipe.PipeSprite;
            currentSprite.color = pipeToCreate.bottomPipe.PipeColor;
            
            Transform topPipe = newPipe.transform.Find("PipeTop");
            currentSprite = topPipe.GetComponent<SpriteRenderer>();
            currentSprite.sprite = pipeToCreate.topPipe.PipeSprite;
            currentSprite.color = pipeToCreate.topPipe.PipeColor;

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
                }

                if (currentPipe.representedPipe.position.x <= DISPOSE_PIPE_POSITION_X)
                {
                    activePipes.RemoveAt(i);
                    pipeObjectPool.ReturnObjectToPool(currentPipe.representedPipe.gameObject);
                    --i;
                }
            }
        }

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