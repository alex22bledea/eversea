using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseOptionsUI();
        });
    }
}
