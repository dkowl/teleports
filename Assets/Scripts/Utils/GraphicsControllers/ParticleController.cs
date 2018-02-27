using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] particleSystems;

    public void Play(int id)
    {
        if (id >= particleSystems.Length)
        {
            Debug.LogWarning("Trying to play sound clip with invalid id");
            return;
        }
        particleSystems[id].Play();
    }

    public void Stop()
    {

    }
}
