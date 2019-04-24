using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Sound_Animation : MonoBehaviour {

    public AudioSource Zombie_Audio;
    public AudioClip[] Zombie_Clips;
	
	void R_Foot_Step() {
        AudioClip clip = Zombie_Clips[0];
        Zombie_Audio.clip = clip;
        Zombie_Audio.Play();
    }
    void L_Foot_Step()
    {
        AudioClip clip = Zombie_Clips[1];
        Zombie_Audio.clip = clip;
        Zombie_Audio.Play();
    }
    void Hit()
    {
        AudioClip clip = Zombie_Clips[2];
        Zombie_Audio.clip = clip;
        Zombie_Audio.Play();
    }
}
