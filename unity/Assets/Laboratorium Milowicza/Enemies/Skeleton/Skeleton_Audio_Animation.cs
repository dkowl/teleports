using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Audio_Animation : MonoBehaviour {

    public AudioSource Skeleton_Audio;
    public AudioClip[] Skeleton_Clips;

    void Foot_Step()
    {
        AudioClip clip = Skeleton_Clips[0];
        Skeleton_Audio.clip = clip;
        Skeleton_Audio.Play();
    }
    
    void Hit()
    {
        AudioClip clip = Skeleton_Clips[1];
        Skeleton_Audio.clip = clip;
        Skeleton_Audio.Play();
    }
}
