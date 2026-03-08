using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderUI : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private int multiplier = 1;

    [Space(12)]
    [SerializeField] private SoundMixerManager.VolumeType volumeType;


    private void Start()
    {
        if (PlayerPrefs.HasKey(volumeType.ToString()))
            volumeSlider.value = PlayerPrefs.GetFloat(volumeType.ToString());
        else
            volumeSlider.value = volumeSlider.maxValue;

        if (volumeSlider.value == volumeSlider.minValue)
            valueText.text = "0";
        else
            valueText.text = ((int)(volumeSlider.value * multiplier / volumeSlider.maxValue * 10f) / 10f).ToString();

        volumeSlider.onValueChanged.AddListener((float value) =>
        {
            if (value == volumeSlider.minValue)
                valueText.text = "0";
            else
                valueText.text = ((int)(value * multiplier / volumeSlider.maxValue * 10f) / 10f).ToString();

            SoundMixerManager.Instance.SetVolume(volumeType, value);

            PlayerPrefs.SetFloat(volumeType.ToString(), value);
        });
    }
}
