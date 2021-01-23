using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlappyBirdPlusPlus
{
    public class BombDisplay : MonoBehaviour
    {
        GameSettings gameSettings;

        [SerializeField] Image progressImage = null;
        [SerializeField] TMP_Text bombCountText = null;

        public void Initialize(GameSettings gameSettings, GameplayManager gameplayManager)
        {
            this.gameSettings = gameSettings;
            gameplayManager.updateBombProgress += HandleBombProgress;
            progressImage.fillAmount = 0f;
            bombCountText.text = "0";
        }

        private void HandleBombProgress(int bombProgress, int bombCount)
        {
            progressImage.fillAmount = bombCount == gameSettings.MaxBombCount ? 1f : (float)bombProgress / gameSettings.ScoreForExtraBomb; // if we have max bombs then there will be no progress towards next bombs
            bombCountText.text = bombCount.ToString();
        }
    }
}