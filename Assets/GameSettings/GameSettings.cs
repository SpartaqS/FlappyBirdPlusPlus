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
    }
}
