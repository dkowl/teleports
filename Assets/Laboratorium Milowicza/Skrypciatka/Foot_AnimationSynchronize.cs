using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot_AnimationSynchronize : MonoBehaviour {

    public ParticleSystem L_Foot_ParticleSystem;
    public ParticleSystem R_Foot_ParticleSystem;
	
	void R_FootStomp () {
        R_Foot_ParticleSystem = GetComponent<ParticleSystem>();
        R_Foot_ParticleSystem.Play();
    }
    void L_FootStomp()
    {
        L_Foot_ParticleSystem = GetComponent<ParticleSystem>();
        L_Foot_ParticleSystem.Play();
    }
}
