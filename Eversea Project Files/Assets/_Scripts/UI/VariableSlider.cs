
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VariableSlider : MonoBehaviour
{
    [SerializeField] private FloatVariableSO variabe;

    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Button changeValButton;

    private void Start()
    {
        UpdateUI();

        changeValButton.onClick.AddListener(() =>
            {
                if (float.TryParse(inputField.text, out float val))
                {
                    variabe.SetValue(val);
                    UpdateUI();
                }
            });
    }

    private void UpdateUI()
    {
        valueText.text = ((int)(variabe.Value * 100) / 100f).ToString();
    }
}
