
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : Singleton<SoundMixerManager>
{
    public enum VolumeType
    {
        MasterVolume,
        SoundFX,
        Music,
    }

    [SerializeField] private AudioMixer audioMixer;

    public void SetVolume(VolumeType volumeType, float volume)
    {
        switch(volumeType)
        {
            case VolumeType.MasterVolume:
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
                break;

            case VolumeType.SoundFX:
                audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(volume) * 20f);
                break;

            case VolumeType.Music:
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
                break;
        }
    }
}
