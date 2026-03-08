using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particleSystems;

    [SerializeField] private VoidEventChannel NightStartedEventChannel;
    [SerializeField] private VoidEventChannel NightEndedEventChannel;

    [SerializeField] private Vector3VariableSO playerPos;

    private void OnEnable()
    {
        NightStartedEventChannel.OnEventRaised += EnableParticles;
        NightEndedEventChannel.OnEventRaised += DisableParticles;
    }

    private void OnDisable()
    {
        NightStartedEventChannel.OnEventRaised -= EnableParticles;
        NightEndedEventChannel.OnEventRaised -= DisableParticles;
    }

    private void Update()
    {
        transform.position = new Vector3(playerPos.Value.x, transform.position.y, playerPos.Value.z);
    }

    private void EnableParticles()
    {
        foreach (ParticleSystem particleSystem in particleSystems)
            particleSystem.Play();
    }

    private void DisableParticles()
    {
        foreach (ParticleSystem particleSystem in particleSystems)
            particleSystem.Pause();
    }
}
