using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devourer_Audio_Animation : MonoBehaviour
{

    public AudioSource Devourer_Body_Audio;
    public AudioSource Devourer_Enviroment_Audio;
    public AudioClip[] Devourer_Clips;

    void Surface_Roar()
    {
        AudioClip clip = Devourer_Clips[0];
        Devourer_Body_Audio.clip = clip;
        Devourer_Body_Audio.Play();
    }

    void Surface()
    {
        AudioClip clip = Devourer_Clips[2];
        Devourer_Enviroment_Audio.clip = clip;
        Devourer_Enviroment_Audio.Play();
    }

    void Death_Roar()
    {
        AudioClip clip = Devourer_Clips[1];
        Devourer_Body_Audio.clip = clip;
        Devourer_Body_Audio.Play();
    }

    void Death_Thump()
    {
        AudioClip clip = Devourer_Clips[4];
        Devourer_Enviroment_Audio.clip = clip;
        Devourer_Enviroment_Audio.Play();
    }

    void Dive()
    {
        AudioClip clip = Devourer_Clips[3];
        Devourer_Enviroment_Audio.clip = clip;
        Devourer_Enviroment_Audio.Play();
    }

    void Hit()
    {
        AudioClip clip = Devourer_Clips[5];
        Devourer_Body_Audio.clip = clip;
        Devourer_Body_Audio.Play();
    }
    void Breath()
    {
        AudioClip clip = Devourer_Clips[6];
        Devourer_Body_Audio.clip = clip;
        Devourer_Body_Audio.Play();
    }
    void Step()
    {
        AudioClip clip = Devourer_Clips[7];
        Devourer_Body_Audio.clip = clip;
        Devourer_Body_Audio.Play();
    }
    void StepDeath()
    {
        AudioClip clip = Devourer_Clips[7];
        Devourer_Enviroment_Audio.clip = clip;
        Devourer_Enviroment_Audio.Play();
    }
    void TailStrikeAudio() {
        AudioClip clip = Devourer_Clips[8];
        Devourer_Body_Audio.clip = clip;
        Devourer_Body_Audio.Play();
    }
    void TeethClack() {
        AudioClip clip = Devourer_Clips[9];
        Devourer_Body_Audio.clip = clip;
        Devourer_Body_Audio.Play();
    }
}
