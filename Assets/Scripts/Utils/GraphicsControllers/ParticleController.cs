using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] particleSystems;

    public void Play(string name)
    {
        GetParticleSystemByName(name)?.Play();
    }

    public void Stop()
    {

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
