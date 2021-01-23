using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlappyBirdPlusPlus
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text scoreText = null;

        public void Initialize(GameplayManager gameplayManager)
        {
            gameplayManager.updateScore += HandleScoreSetting;
            scoreText.text = "0";
        }

        private void HandleScoreSetting(int currentScore)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}