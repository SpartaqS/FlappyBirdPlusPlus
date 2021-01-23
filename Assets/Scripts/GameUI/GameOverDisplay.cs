using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlappyBirdPlusPlus
{
    public class GameOverDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text finalScoreText = null;        

        public void Initialize(GameplayManager gameplayManager)
        {
            gameplayManager.announceEndGame += HandleShowEndGame;
        }

        private void HandleShowEndGame(int finalScore)
        {
            gameObject.SetActive(true);
            finalScoreText.text = finalScore.ToString();
        }
    }
}