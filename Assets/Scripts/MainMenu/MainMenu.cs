using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlappyBirdPlusPlus
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Main Menu")]
        [SerializeField] Button playButton = null;
        [SerializeField] Button seeHighscoreButton = null;
        [Header("Highscore Panel")]
        bool highscoresLoaded = false;
        [SerializeField] RectTransform highscorePanel = null;
        [SerializeField] GameObject higscoreDisplayPrefab = null;
        [SerializeField] Transform highscoreContent = null;
        [SerializeField] Button closeHighscoreButton = null;



        private void Awake()
        {
            highscorePanel.transform.position += Vector3.up * Screen.height; // move the panel out of the way
            DisableHigscorePanel();
            highscoresLoaded = false;
            SetMainMenuButtonsInteractability(true);
        }
        //LeanTween.scale(highscoreText.rectTransform, Vector3.one, 1f).setEase(LeanTweenType.easeOutBounce).setDelay(1f); ; // show whether we got the higscore or not
        //LeanTween.scale(playAgainButton, Vector3.one, 1f).setEase(LeanTweenType.easeOutBounce).setDelay(1.5f); ; // show whether we got the higscore or no

        #region Buttons
        public void ButtonStart()
        {
            SetMainMenuButtonsInteractability(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }

        public void ButtonHighscores()
        {
            SetMainMenuButtonsInteractability(false);
            if (!highscoresLoaded) // we need to load the highscores only once  (the only way to change highscore is to play the game and that requires unloading of the MainMenu scene)
            {
                List<int> highscores = SaveSystem.ReadHighscores();

                for (int i = 0; i < highscores.Count; i++)
                {
                    GameObject currentDisplay = Instantiate(higscoreDisplayPrefab, highscoreContent);
                    currentDisplay.transform.Find("Text_Position").GetComponent<TMP_Text>().text = "#" + (i + 1);
                    currentDisplay.transform.Find("Text_Score").GetComponent<TMP_Text>().text = highscores[i].ToString();
                }
                highscoresLoaded = true;
            }

            highscorePanel.gameObject.SetActive(true); // only activate the panel but keep the return button deactivated
            LeanTween.moveY(highscorePanel.gameObject, highscorePanel.position.y - Screen.height, 1f).setEase(LeanTweenType.easeOutBounce).setOnComplete(x => closeHighscoreButton.interactable = true);
        }

        public void ButtonCloseHighscores()
        {
            LeanTween.moveY(highscorePanel.gameObject, highscorePanel.position.y + Screen.height, 1f).setEase(LeanTweenType.easeInBounce).setOnComplete(x => DisableHigscorePanel());
        }

        private void DisableHigscorePanel()
        {
            highscorePanel.gameObject.SetActive(false);
            closeHighscoreButton.interactable = false;
            SetMainMenuButtonsInteractability(true);
        }

        private void SetMainMenuButtonsInteractability(bool targetValue)
        {
            playButton.interactable = targetValue;
            seeHighscoreButton.interactable = targetValue;
        }

        #endregion
    }
}