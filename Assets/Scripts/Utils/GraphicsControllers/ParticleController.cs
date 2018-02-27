using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particleSystems;

    public void Play(string psName)
    {
        GetParticleSystemByName(psName)?.Play();
    }

    public void Stop(string psName)
    {
        GetParticleSystemByName(psName)?.Stop();
    }

    private ParticleSystem GetParticleSystemByName(string particleSystemName)
    {
        foreach (var currentParticleSystem in particleSystems)
        {
            if (particleSystemName == currentParticleSystem.name)
            {
                return currentParticleSystem;
            }
        }
        Debug.LogWarningFormat("Particle system named {0} not found", particleSystemName);
        return null;
    }
}
