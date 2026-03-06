using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class StartScreenUI : MonoBehaviour
    {
        private MainMenuUI MainMenuUI => MainMenuUI.Instance;
        
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private Button returnButton;
        
        private CanvasGroup canvasGroup;
        
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        private void OnEnable()
        {
            playButton.onClick.AddListener(PlayGame);
            returnButton.onClick.AddListener(ReturnToMainMenu);
            MainMenuUI.OnStartGame += ShowStartScreen;
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveAllListeners();
            returnButton.onClick.RemoveAllListeners();
            MainMenuUI.OnStartGame -= ShowStartScreen;
        }

        private void ShowStartScreen()
        {
            MainMenuUI.Show(canvasGroup);
        }

        private void ReturnToMainMenu()
        {
            MainMenuUI.Hide(canvasGroup);
            MainMenuUI.ReturnToMainMenu();
        }

        private void PlayGame()
        {
            Cursor.visible = false;
            SceneManager.LoadScene(1);
        }
    }
}