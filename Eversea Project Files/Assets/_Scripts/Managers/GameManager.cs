using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action OnGamePaused;
    public event Action OnGameUnpaused;

    [SerializeField] private FloatVariableSO baseTimeScale;

    public bool IsPaused { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Time.timeScale = baseTimeScale.Value;
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke();
        }
        else
        {
            Time.timeScale = baseTimeScale.Value;
            OnGameUnpaused?.Invoke();
        }
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        OnGamePaused?.Invoke();
    }

    public void UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = baseTimeScale.Value;
        OnGameUnpaused?.Invoke();
    }
}
