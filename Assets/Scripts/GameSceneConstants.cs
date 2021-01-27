using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdPlusPlus
{
    public static class GameSceneConstants
    {
        // World constants
        public const float CAMERA_SIZE = 100f; // better to store it here than call Camera.Main

        public const float MAX_PIPE_Y = 88f;
        public const float MIN_PIPE_Y = -63f;

        public const float PIPE_WIDTH = 26f;
        public const float PIPE_HEIGHT = 160f;

        public const float DISPOSE_PIPE_POSITION_X = -100f;
        public const float SPAWN_PIPE_POSITION_X = 100f;

        public const float EXPLOSION_MAGNITUDE = 10000f;

        // Bird constants

        public const float BIRD_TWEEN_IN_TIME = 1f;
    }
}