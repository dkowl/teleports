using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devourer_Audio_Animation : MonoBehaviour {

    public AudioSource Devourer_Audio;
    public AudioClip[] Devourer_Clips;

    void Surface_Roar() {
        AudioClip clip = Devourer_Clips[0];
        Devourer_Audio.clip = clip;
        Devourer_Audio.Play();
    }

    void Death_Roar()
    {
        AudioClip clip = Devourer_Clips[1];
        Devourer_Audio.clip = clip;
        Devourer_Audio.Play();
    }
}
