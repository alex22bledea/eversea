
using TMPro;
using UnityEngine;

public class KeybindDesplayButton : MonoBehaviour
{
    [SerializeField] private InputReaderSO input;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Space(12)]
    [SerializeField] private InputReaderSO.Binding binding;

    private void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        buttonText.text = input.GetBindingText(binding);
    }
}
