using UnityEngine;
using TMPro;

public class MainInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI moneyText;

    const int HOUR_DURATION = 60;

    private void Update()
    {
        dayText.text = "DAY: " + TimeSystem.Instance.daysSurvived.ToString();

        float crTime = TimeSystem.Instance?.GetCurrentTime() ?? 0;
        int hour = (int)crTime / HOUR_DURATION;
        string suf = hour >= 12 ? " PM" : " AM";
        hour %= 12; /// from 0 to 11  
        int minute = (int)crTime % HOUR_DURATION;
        minute -= minute % 10;
        string minutePref = minute <= 9 ? ":0" : ":";
        timeText.text = hour.ToString() + minutePref + minute.ToString() + suf;

        float crMoney = UpgradeSystem.Instance?.Money ?? 0f;
        moneyText.text = ((int)crMoney).ToString();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
