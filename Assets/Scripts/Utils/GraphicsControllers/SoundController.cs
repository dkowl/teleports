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
        ValidateClips();
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void Play(string clipName)
    {
        foreach (var clip in clips)
        {
            if (clip.name != clipName) continue;
            PlayClip(clip);
            return;
        }
        Debug.LogWarning("Trying to play a sound clip with an invalid name");
    }

    public void ValidateClips()
    {
        foreach (var clip in clips)
        {
            if (clip == null)
            {
                Debug.LogError("Something went wrong, there is a null audio clip");
            }
        }
    }
}
