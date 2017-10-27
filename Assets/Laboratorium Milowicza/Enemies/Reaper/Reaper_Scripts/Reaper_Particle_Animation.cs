using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_Particle_Animation : MonoBehaviour {

    public ParticleSystem TerrifyParticleSystem;
    public ParticleSystem PlantScytheParticleSystem;
    public ParticleSystem DeathParticleSystem;
 
    void Terrify()
    {
        TerrifyParticleSystem.Play();
    }
    void PlantScythe()
    {
        PlantScytheParticleSystem.Play();
    }
    void Ghosts() {
        DeathParticleSystem.Play();
    }
}
