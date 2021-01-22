using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    public class GameplayManager : MonoBehaviour
    {
        List<Pipe> pipeTypes = new List<Pipe>();
        GameObject playerObject = null;
        GameObject pipePrefab = null;

        int score;

        public void Initialize(GameObject playerObject, List<Pipe> pipeTypes, GameObject pipePrefab)
        {
            this.playerObject = playerObject;
            this.pipeTypes = pipeTypes;
            this.pipePrefab = pipePrefab;

            Debug.Log("Loaded " + pipeTypes.Count + " distinct pipe types");

            score = 0;

            CreatePipe(10);
        }


        private void CreatePipe(int x)
        {
            Pipe pipeToCreate = ObtainPipe();

            GameObject newPipe = Instantiate(pipePrefab, transform);

            Transform bottomPipe = newPipe.transform.Find("PipeBottom");
            bottomPipe.GetComponent<SpriteRenderer>().sprite = pipeToCreate.bottomPipe.PipeSprite;
            Transform topPipe = newPipe.transform.Find("PipeTop");
            topPipe.GetComponent<SpriteRenderer>().sprite = pipeToCreate.topPipe.PipeSprite;
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

            int selectedPipeIndex = Random.Range(0, avaliablePipes.Count - 1);

            return avaliablePipes[selectedPipeIndex];
        }
    }
}