using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    
    private void OnEnable()
    {
        continueButton.onClick.AddListener(LoadMainMenuScene);
    }

    private void OnDisable()
    {
        continueButton.onClick.RemoveAllListeners();
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
