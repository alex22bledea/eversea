
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOptionsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject mainMenuUI;

    private void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            mainMenuUI.SetActive(true);
            this.gameObject.SetActive(false);
        });
    }
}
