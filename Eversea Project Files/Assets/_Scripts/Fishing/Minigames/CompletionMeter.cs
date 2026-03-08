using UnityEngine;
using UnityEngine.UI;
using System;

public class CompletionMeter : MonoBehaviour
{
    public event Action OnCompletion;

    [SerializeField] private Image fillImg;

    private float _fillAmount;
    private float FillAmount {
        get => _fillAmount;
        set => fillImg.fillAmount = _fillAmount = Mathf.Clamp01(value);
    }

    private float multiplier = 1f;

    [SerializeField, Tooltip("value in seconds")]
    private FloatVariableSO fishingMinigameDuration;

    private bool isStarted = false;

    private void Update()
    {
        if (isStarted)
        {
            FillAmount += multiplier / fishingMinigameDuration.Value * Time.deltaTime;
            if (FillAmount == 1f)
            {
                OnCompletion?.Invoke();
                FillAmount = 0f;
                isStarted = false;
            }
        }
    }

    public void PrepareCompMeter()
    {
        FillAmount = 0f;
    }

    public void StartMeter()
    {
        isStarted = true;
    }

    public void CancellMeter()
    {
        FillAmount = 0f;
        isStarted = false;
    }

    public void Increase(float value) => FillAmount += value;

    public void SetMultiplier(float value) => multiplier = value;
    
}
