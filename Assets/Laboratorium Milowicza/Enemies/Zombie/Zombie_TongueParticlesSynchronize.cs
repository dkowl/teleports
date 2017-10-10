using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_TongueParticlesSynchronize : MonoBehaviour {

    public ParticleSystem TongueParticleSystem;

    void Lick () {
        TongueParticleSystem.Play();
    }
}
