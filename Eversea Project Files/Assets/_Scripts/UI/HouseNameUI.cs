
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HouseNameUI : MonoBehaviour
{
    [SerializeField] private PlayerReference playerReference;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        Hide();
        UIManager.Instance.OnWindowChanged += UIManager_OnWindowChanged;
    }

    private void UIManager_OnWindowChanged(UIManager.UIWindow activeWindow)
    {
        if (activeWindow == UIManager.UIWindow.None && playerReference.Value.IsDocked)
            Show();
        else
            Hide();
    }

    private void Hide()
    {
        image.enabled = false;
        text.enabled = false;
    }

    private void Show()
    {
        image.enabled = true;
        text.enabled = true;
    }
}
