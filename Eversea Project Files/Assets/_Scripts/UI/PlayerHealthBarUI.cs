
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private PlayerReference playerReference;

    [SerializeField] private Image healthBar;

    private HealthSystem playerHealthSystem;

    private void Start()
    {
        playerHealthSystem = playerReference.Value.GetComponent<HealthSystem>();

        playerHealthSystem.OnHealthChanged += Player_OnHealthChanged;
        UpdateUI();
    }

    private void Player_OnHealthChanged(float changedAmount)
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        if (playerHealthSystem != null)
            UpdateUI();
    }

    private void UpdateUI()
    {
        healthBar.fillAmount = playerHealthSystem.HealthNormalized;
    }
}
