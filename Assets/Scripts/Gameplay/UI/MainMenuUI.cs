using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    [DefaultExecutionOrder(-2)]
    public class MainMenuUI : MonoBehaviour
    {
        public static MainMenuUI Instance;
        
        [SerializeField]
        private CanvasGroup buttonCanvasGroup;
        
        
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button settingsButton;
        [SerializeField]
        private Button creditsButton;
        [SerializeField]
        private Button quitButton;

        public event Action OnStartGame;
        
        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            Instance = this;
        }

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
            Hide(buttonCanvasGroup);
            OnStartGame?.Invoke();
        }

        public void ReturnToMainMenu()
        {
            Show(buttonCanvasGroup);
        }
        
        public void Show(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void Hide(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}