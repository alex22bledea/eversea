using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindButtonUI : MonoBehaviour
{
    [SerializeField] private InputReaderSO input;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject pressToRebindBindingScreen;

    [Space(12)]
    [SerializeField] private InputReaderSO.Binding binding;

    private void OnEnable()
    {
        UpdateUI();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            ShowRebindScreen();
            input.RebindBinding(binding, () =>
            {
                HideRebindScreen();
                UpdateUI();
            });
        });
    }

    private void ShowRebindScreen() => pressToRebindBindingScreen.SetActive(true);

    private void HideRebindScreen() => pressToRebindBindingScreen.SetActive(false);

    private void UpdateUI()
    {
        buttonText.text = input.GetBindingText(binding);
    }
}
