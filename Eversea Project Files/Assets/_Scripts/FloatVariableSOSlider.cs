
using UnityEngine;
using UnityEngine.UI;

public class FloatVariableSOSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private FloatVariableSO variable;

    private void OnEnable()
    {
        slider.value = variable.Value;
    }

    private void Start()
    {
        slider.onValueChanged.AddListener((float val) =>
        {
            variable.SetValue(val);
        });
    }
}
