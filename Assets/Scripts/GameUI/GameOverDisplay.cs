﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlappyBirdPlusPlus
{
    public class GameOverDisplay : MonoBehaviour
    {
        [SerializeField] Image backgroundImage = null;
        [SerializeField] GameObject boxGameObject = null;
        [SerializeField] TMP_Text finalScoreText = null;
        [SerializeField] TMP_Text highscoreText = null;
        [SerializeField] Color noHighscoreColor = new Color();
        [SerializeField] Color newHighscoreColor = new Color();

        [SerializeField] RectTransform playAgainButton = null;
        [SerializeField] RectTransform mainMenuButton = null;

        public void Initialize(GameplayManager gameplayManager)
        {
            gameplayManager.announceEndGame += HandleShowEndGame;
            boxGameObject.transform.localPosition = new Vector3(0f, -Screen.height, 0);
            // set aplha of the background to 0 so that it can fade in
            Color temp = backgroundImage.color;
            temp.a = 0f;
            backgroundImage.color = temp;

            finalScoreText.rectTransform.localScale = Vector3.zero; // hide the score text
            highscoreText.rectTransform.localScale = Vector3.zero; // hide the information about the highscore
            playAgainButton.localScale = Vector3.zero; // hide the button
            mainMenuButton.localScale = Vector3.zero; // hide the button
        }

        private void HandleShowEndGame(int finalScore)
        {
            LeanTween.alpha(backgroundImage.rectTransform, 0.5f, 0.5f).setRecursive(false); // setRecursive(false) so we don't tween the alpha on other child images
            LeanTween.moveY(boxGameObject, Screen.height/2, 1f).setEase(LeanTweenType.easeOutBounce);            
            gameObject.SetActive(true);
            finalScoreText.text = finalScore.ToString();

            CheckForHighscore(finalScore);
        }

        private void CheckForHighscore(int finalScore)
        {
            List<int> highscores = SaveSystem.ReadHighscores();
            bool hasImprovedHighscores = false;

            for (int i = 0; i < highscores.Count; i++)
            {
                if (highscores[i] < finalScore)
                {
                    hasImprovedHighscores = true;
                    highscores.Insert(i, finalScore); //add score
                    if (highscores.Count > SaveSystem.HIGHSCORE_COUNT) // remove excess highscores (should be only 1 to remove but let's suppose that there is an update and the highscore count has been decreased
                    {
                        while (highscores.Count > SaveSystem.HIGHSCORE_COUNT)
                        {
                            highscores.RemoveAt(highscores.Count - 1);
                        }
                    }
                    highscoreText.text = "Highscore #" + (i+1) + "!"; // inform the player which spot on the highscore list they have achieved
                    highscoreText.color = newHighscoreColor;

                    i += highscores.Count;// exit out of the loop
                }
            }

            SaveSystem.SaveHighscores(highscores); // save the new set of highscores

            if (!hasImprovedHighscores) // if did not get a highscore, then give this message
            {
                highscoreText.text = "Better luck next time!";
                highscoreText.color = noHighscoreColor;
            }

            LeanTween.scale(finalScoreText.rectTransform, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(0.75f); // show the score text
            LeanTween.scale(highscoreText.rectTransform, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(1f); ; // show whether we got the higscore or not
            LeanTween.scale(playAgainButton, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(1.5f); ; // show the replay button
            LeanTween.scale(mainMenuButton, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(1.75f); ; // show the main menu button
        }

        #region Buttons
        public void ButtonStartAgain()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }

        public void ButtonMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        #endregion
    }
}