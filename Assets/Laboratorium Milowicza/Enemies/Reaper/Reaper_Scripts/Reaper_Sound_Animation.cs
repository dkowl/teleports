using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_Sound_Animation : MonoBehaviour {

    public AudioClip[] Reaper_Sounds;
    public AudioSource Reaper_Audio;


    void ScytheAttack() {
        AudioClip clip = Reaper_Sounds[0];
        Reaper_Audio.clip = clip;
        Reaper_Audio.Play();
    }
}
