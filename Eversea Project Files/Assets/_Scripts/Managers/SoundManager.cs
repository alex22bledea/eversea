
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource AudioSourcePrefab;

    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip coinSound2;
    [SerializeField] private AudioClip cashSound;

    private void Start()
    {
        if (FishingMinigamesManager.Instance != null)
        {
            FishingMinigamesManager.Instance.OnFishingGameStarted += FishingMinigamesManager_OnFishingGameStarted;
            FishingMinigamesManager.Instance.OnFishingMinigameEnded += BaseFishingMinigame_OnMinigameFinished;
        }
        if(InventoryManager.Instance != null)
            InventoryManager.Instance.OnAnyItemSold += InventoryManager_OnAnyItemSold;
    }

    private void InventoryManager_OnAnyItemSold()
    {
        PlaySoundClip(cashSound);
    }

    private void FishingMinigamesManager_OnFishingGameStarted()
    {
        PlaySoundClip(coinSound2);
    }
    private void BaseFishingMinigame_OnMinigameFinished()
    {
        PlaySoundClip(coinSound);
    }

    public void PlaySoundClip(AudioClip audioClip, float volume = 1f)
    {
        AudioSource audioSource = Instantiate(AudioSourcePrefab, Camera.main.transform);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        audioSource.pitch *= Random.Range(0.9f, 1.1f);

        float clipLength = audioClip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
