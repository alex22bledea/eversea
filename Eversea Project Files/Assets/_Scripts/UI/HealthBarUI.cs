using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    [Header("Components")]
    [SerializeField] private Image background;
    [SerializeField] private Image fill;

    [Space(12), Header("Vars")]
    [SerializeField] private bool hideUntilDamaged;

    private void Start()
    {
        healthSystem.OnHealthChanged += SetFill;

        fill.fillAmount = healthSystem.HealthNormalized;

        if(hideUntilDamaged && healthSystem.HealthNormalized == 1f)
            gameObject.SetActive(false);
    }

    private void SetFill(float value)
    {
        fill.fillAmount = healthSystem.HealthNormalized;
        gameObject.SetActive(true);
    }
}
