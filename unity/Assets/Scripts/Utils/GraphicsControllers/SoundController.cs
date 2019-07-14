using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;
    private Unit unit;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ValidateClips();
    }

    private void Start()
    {
        unit = GetComponentInParent<Unit>();
        if(unit != null) unit.CastingState.resetCastEvent += Stop;
    }

    private void OnDestroy()
    {
        if(unit != null) unit.CastingState.resetCastEvent -= Stop;
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

    private void Stop(CastingState.CastEventArgs args)
    {
        Stop();
    }

    public void Stop()
    {
        audioSource.Stop();
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
