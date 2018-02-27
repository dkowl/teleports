using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(int id)
    {
        if (id >= clips.Length)
        {
            Debug.LogWarning("Trying to play sound clip with invalid id");
            return;
        }
        audioSource.clip = clips[id];
        audioSource.Play();
    }
}
