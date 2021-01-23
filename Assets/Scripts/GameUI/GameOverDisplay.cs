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
        [SerializeField] GameObject boxGameObject = null;

        public void Initialize(GameplayManager gameplayManager)
        {
            gameplayManager.announceEndGame += HandleShowEndGame;
            boxGameObject.transform.localPosition = new Vector3(0f, -Screen.height, 0);
            // set aplha of the background to 0 so that it can fade in
            Image backgroundImage = GetComponent<Image>();
            Color temp = backgroundImage.color;
            temp.a = 0f;
            backgroundImage.color = temp;
        }

        private void HandleShowEndGame(int finalScore)
        {
            LeanTween.alpha(GetComponent<Image>().rectTransform, 0.5f, 0.5f);
            LeanTween.moveY(boxGameObject, Screen.height/2, 1f).setEase(LeanTweenType.easeOutBounce);            
            gameObject.SetActive(true);
            finalScoreText.text = finalScore.ToString();
        }
    }
}