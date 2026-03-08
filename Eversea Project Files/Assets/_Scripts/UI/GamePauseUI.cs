using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ToggleGamePaused();
        });

        optionsButton.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenOptionsUI();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Scene.MainMenuScene);
        });
    }
}
