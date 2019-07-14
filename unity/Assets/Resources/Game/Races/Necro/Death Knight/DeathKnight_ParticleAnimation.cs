using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKnight_ParticleAnimation : MonoBehaviour {

    public ParticleSystem L_Foot_ParticleSystem;
    public ParticleSystem R_Foot_ParticleSystem;
    public ParticleSystem L_Eye_ParticleSystem;
    public ParticleSystem R_Eye_ParticleSystem;

    void R_FootStomp () {
        R_Foot_ParticleSystem.Play();
    }
    void L_FootStomp()
    {
        L_Foot_ParticleSystem.Play();
    }
    void EyeActivate() {
        L_Eye_ParticleSystem.Play();
        R_Eye_ParticleSystem.Play();
    }
    void EyeDeactivate() {
        L_Eye_ParticleSystem.Stop();
        R_Eye_ParticleSystem.Stop();
    }
}
