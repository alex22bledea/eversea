using System;
using UnityEngine;

public abstract class BaseFishingMinigame : MonoBehaviour
{
    [SerializeField] protected InputReaderSO input;

    [Header("Comp Meter")]
    [SerializeField] protected CompletionMeter compMeter;

    protected bool isStarted = false;

    public virtual void PrepareMinigame()
    {
        isStarted = false;
    }

    public virtual void StartMinigame()
    {
        compMeter.StartMeter();
        isStarted = true;
    }

    public void StopMinigame()
    {
        isStarted = false;
    }
}
