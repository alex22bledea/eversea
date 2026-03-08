
using System;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigamesManager : Singleton<FishingMinigamesManager>
{
    public event Action OnFishingGamePrepared;
    public event Action OnFishingGameStarted;
    public event Action OnFishingMinigameEnded;

    [SerializeField] private InputReaderSO input;

    [Space(12), Header("Minigames Array")]
    [SerializeField] private BaseFishingMinigame[] fishingMinigameArray;

    public ItemData CurrentItem { get; private set; }

    private BaseFishingMinigame selectedMinigame;
    public FishingSpot SelectedFishingSpot { get; private set; }

    [SerializeField] private CompletionMeter completionMeter;

    public bool IsPlayingMinigame { get; private set; }

    private void Start()
    {
        completionMeter.OnCompletion += FinishMinigame;
    }

    private void OnEnable()
    {
        input.FishEvent += TryStartMinigameForFishingSpot;
    }

    private void OnDisable()
    {
        input.FishEvent -= TryStartMinigameForFishingSpot;
    }

    public void SetSelectedFishingSpot(FishingSpot fishingSpot)
    {
        SelectedFishingSpot = fishingSpot;
        if (selectedMinigame != null)
            selectedMinigame.gameObject.SetActive(false);

        selectedMinigame = fishingMinigameArray.GetRandomElement();
        selectedMinigame.gameObject.SetActive(true);
    }

    public int GetSelectedFishingSpotAvailableFish() => SelectedFishingSpot.FishAvailable;

    private void FinishMinigame()
    {
        RewardPlayer();
        EndCurrentMinigame();
    }

    public void CancellMinigame()
    {
        completionMeter.CancellMeter();
        EndCurrentMinigame();
    }

    private void TryStartMinigameForFishingSpot()
    {
        if (UIManager.Instance.activeUIWindow != UIManager.UIWindow.Fishing)
            return;

        if (IsPlayingMinigame)
            return;

        if (SelectedFishingSpot.FishAvailable == 0)
            return;

        if (InventoryManager.Instance.HasItemInHand())
            return;

        selectedMinigame.StartMinigame();
        OnFishingGameStarted?.Invoke();

        IsPlayingMinigame = true;
    }

    public void PrepareMinigame()
    {
        if (SelectedFishingSpot.FishAvailable >= 1)
        {
            CurrentItem = SelectedFishingSpot.GetNextItem();

            selectedMinigame.PrepareMinigame();
            completionMeter.PrepareCompMeter();
        }
        else
        {
            CurrentItem = null;

            selectedMinigame.gameObject.SetActive(false);
            selectedMinigame = null;
        }

        OnFishingGamePrepared?.Invoke();
    }

    private void RewardPlayer()
    {
        InventoryManager.Instance.GiveItemInHand(CurrentItem);
        CurrentItem = null;
    }

    private void EndCurrentMinigame()
    {
        selectedMinigame.StopMinigame();
        IsPlayingMinigame = false;
        SelectedFishingSpot.DecreaseFishAvailable();

        PrepareMinigame();

        OnFishingMinigameEnded?.Invoke();
    }

}
