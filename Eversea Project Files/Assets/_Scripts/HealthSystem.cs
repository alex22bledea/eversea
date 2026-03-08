using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDeath;
    /// <summary> Sends the amount of health changed </summary>
    public event Action<float> OnHealthChanged;

    [SerializeField] private FloatVariableSO maxHealth;

    private float _health;
    private float Health {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0f, maxHealth.Value);
            HealthNormalized = _health / maxHealth.Value;
        }
    }

    public float HealthNormalized { get; private set; }

    private void Awake()
    {
        Health = maxHealth.Value;
    }

    public void TakeDamage(float value)
    {
        Health -= value;

        if (value != 0f)
            OnHealthChanged?.Invoke(-value);

        if (Health <= 0f)
            OnDeath?.Invoke();
    }

    public void HealToFull()
    {
        if (Health == maxHealth.Value)
            return;

        float healAmount = maxHealth.Value - Health;

        Health = maxHealth.Value;
        OnHealthChanged?.Invoke(healAmount);
    }
}
