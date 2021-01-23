using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] GameObject playerPrefab = null;
        public GameObject PlayerPrefab { get => playerPrefab; }

        [SerializeField] float speed = 30f;
        public float Speed { get => speed; }

        [SerializeField] float gravityScale = 20f;
        public float GravityScale { get => gravityScale; }

        [SerializeField] float pipeGapSize = 50f;
        public float PipeGapSize { get => pipeGapSize; }

        [SerializeField] float pipeSpawnTimer = 3f;
        public float PipeSpawnTimer { get => pipeSpawnTimer; }

        [SerializeField] int scoreForExtraBomb = 2;
        public int ScoreForExtraBomb { get => scoreForExtraBomb; }

        [SerializeField] int maxBombCount = 3;
        public int MaxBombCount { get => maxBombCount; }
    }
}
