using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button settingsButton;
        [SerializeField]
        private Button creditsButton;
        [SerializeField]
        private Button quitButton;

        public void OnEnable()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            creditsButton.onClick.AddListener(OnCreditsButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
            creditsButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }

        private void OnQuitButtonClicked()
        {
            Application.Quit();
        }

        private void OnCreditsButtonClicked()
        {
            
        }

        private void OnSettingsButtonClicked()
        {
            
        }

        private void OnStartButtonClicked()
        {
            SceneManager.LoadScene(1);
        }
    }
}