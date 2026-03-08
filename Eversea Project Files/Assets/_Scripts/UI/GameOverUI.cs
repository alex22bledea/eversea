
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI daysSurvivedText;

    private void OnEnable()
    {
        daysSurvivedText.text = "DAYS SURVIVED: " +  TimeSystem.Instance.daysSurvived.ToString();
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() =>
            {
                Loader.LoadScene(Loader.Scene.MainMenuScene);
            });
    }
}
