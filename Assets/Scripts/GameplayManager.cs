using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    public class GameplayManager : MonoBehaviour
    {
        List<Pipe> pipeTypes = new List<Pipe>();
        GameObject playerObject = null;

        public void Initialized(GameObject playerObject, List<Pipe> pipeTypes)
        {
            this.playerObject = playerObject;
            this.pipeTypes = pipeTypes;
        }
    }
}