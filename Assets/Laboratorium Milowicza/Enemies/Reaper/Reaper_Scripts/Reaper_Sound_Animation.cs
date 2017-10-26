using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_Sound_Animation : MonoBehaviour {

    public AudioSource Reaper_Audio;
    public AudioClip[] Reaper_Clips;

    void ScytheAttack() {
        AudioClip clip = Reaper_Clips[0];
        Reaper_Audio.clip = clip;
        Reaper_Audio.Play();
    }
    void TerrifySound() {
        AudioClip clip = Reaper_Clips[1];
        Reaper_Audio.clip = clip;
        Reaper_Audio.Play();
    }
    void ClothFlap() {
        AudioClip clip = Reaper_Clips[2];
        Reaper_Audio.clip = clip;
        Reaper_Audio.Play();
    }
}
