using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        [Header("Player")]
        [SerializeField] GameObject playerPrefab = null;
        public GameObject PlayerPrefab { get => playerPrefab; }

        [SerializeField] Color birdColor = new Color(0f, 0f, 0f);
        public Color BirdColor { get => birdColor; }

        [SerializeField] float speed = 30f;
        public float Speed { get => speed; }

        [SerializeField] float flapVelocity = 100f;
        public float FlapVelocity { get => flapVelocity; }

        [SerializeField] float gravityScale = 20f;
        public float GravityScale { get => gravityScale; }

        [Header("Pipes")]
        [SerializeField] float pipeGapSize = 50f;
        public float PipeGapSize { get => pipeGapSize; }

        [SerializeField] float pipeSpawnTimer = 3f;
        public float PipeSpawnTimer { get => pipeSpawnTimer; }
        [Header("Bombs")]
        [SerializeField] int scoreForExtraBomb = 2;
        public int ScoreForExtraBomb { get => scoreForExtraBomb; }

        [SerializeField] int maxBombCount = 3;
        public int MaxBombCount { get => maxBombCount; }

        public void ApplyRemoteConfig(Color birdColor)
        {
            this.birdColor = birdColor;
        }
    }
}
