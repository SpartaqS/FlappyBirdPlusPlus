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
        [SerializeField] RectTransform titleTransform = null;
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

            titleTransform.localScale = Vector3.zero;
            playButton.gameObject.transform.localScale = Vector3.zero;
            seeHighscoreButton.gameObject.transform.localScale = Vector3.zero;
            ShowMainMenuButtons();

        }

        #region Buttons

        public void ButtonStart()
        {
            HideMainMenuButtons();
            //SetMainMenuButtonsInteractability(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }

        public void ButtonHighscores()
        {
            HideMainMenuButtons();
            //SetMainMenuButtonsInteractability(false);
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
            closeHighscoreButton.interactable = false;
            LeanTween.moveY(highscorePanel.gameObject, highscorePanel.position.y + Screen.height, 1f).setEase(LeanTweenType.easeInBounce).setOnComplete(x => DisableHigscorePanel());
        }

        private void DisableHigscorePanel()
        {
            highscorePanel.gameObject.SetActive(false);
            ShowMainMenuButtons();
            //SetMainMenuButtonsInteractability(true);
        }

        private void ShowMainMenuButtons()
        {
            LeanTween.scale(titleTransform.gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic);
            LeanTween.scale(playButton.gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(0.25f);
            LeanTween.scale(seeHighscoreButton.gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(0.5f);
            SetMainMenuButtonsInteractability(true);
        }

        private void HideMainMenuButtons()
        {
            LeanTween.scale(titleTransform.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.linear);
            LeanTween.scale(playButton.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.linear);
            LeanTween.scale(seeHighscoreButton.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.linear);
            SetMainMenuButtonsInteractability(false);
        }

        private void SetMainMenuButtonsInteractability(bool targetValue)
        {
            playButton.interactable = targetValue;
            seeHighscoreButton.interactable = targetValue;
        }

        #endregion
    }
}