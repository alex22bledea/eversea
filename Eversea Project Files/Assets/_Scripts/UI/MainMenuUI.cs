using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Space(12)]
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button testingButton;

    [Space(12)]
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject testingUI;


    private void Start()
    {
        playButton.onClick.AddListener(() =>
            {
                Loader.LoadScene(Loader.Scene.GameScene);
            });

        settingsButton.onClick.AddListener(() =>
            {
                optionsUI.SetActive(true);
                this.gameObject.SetActive(false);
            });

        quitButton.onClick.AddListener(() =>
            {
                Debug.Log("Application Quit");
                Application.Quit();
            });

        tutorialButton.onClick.AddListener(() =>
        {
            tutorialUI.SetActive(true);
            this.gameObject.SetActive(false);
        });

        testingButton.onClick.AddListener(() =>
        {
            testingUI.SetActive(true);
            this.gameObject.SetActive(false);
        });
    }
}
