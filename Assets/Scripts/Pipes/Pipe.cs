using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    [CreateAssetMenu(fileName = "Pipe_Default", menuName = "GameCustomization/Pipe")]
    public class Pipe : ScriptableObject
    {
        [Header("Visuals")]

        public PipeVisuals topPipe;
        public PipeVisuals bottomPipe;

        [Header("Spawning")]
        [SerializeField] [Tooltip("The lowest score that the player has to achieve so that this pipe can appear")] [Min(0)]
        private int minimumScore = 0;
        public int MinimumScore { get => minimumScore; }

        [SerializeField] [Tooltip("Should there be an upper limit to the appearence of this pipe?")]
        private bool restrictMaximumScore = false;
        public bool RestrictMaximumScore { get => restrictMaximumScore; }
        [SerializeField] [Tooltip("If the player achieves this score, the pipe will no longer appear")] [Min(0)]
        private int maximumScore = int.MaxValue; // make this "tickable" (if not visible then set to "ininity")
        public int MaximumScore { get => maximumScore; set => maximumScore = value; }
        /*
        [SerializeField] [Tooltip("Should this pipe have custom chance of spawning?")]
        private bool useCustomSpawnWeight = false;
        public bool UseCustomSpawnWeight { get => useCustomSpawnWeight; }
        [SerializeField] [Tooltip("How likely should this pipe configuration appear if there are many possible")] [Min(1)]
        private int weight = 1; 
        public int Weight { get => weight; }
        */
        [System.Serializable]
        public class PipeVisuals
        {
            [SerializeField]
            [Tooltip("The texture of the pipe")]
            private Sprite pipeSprite;
            public Sprite PipeSprite { get => pipeSprite; set => pipeSprite = value; }

            [SerializeField]
            [Tooltip("The color/tint of the pipe")]
            private Color32 pipeColor = new Color32(255, 255, 255, 255);
            public Color32 PipeColor { get => pipeColor; }
        }

    }
}
