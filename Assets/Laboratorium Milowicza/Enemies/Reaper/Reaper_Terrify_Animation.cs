using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_Terrify_Animation : MonoBehaviour {

    public ParticleSystem TerrifyParticleSystem;

 
    void Terrify()
    {
        TerrifyParticleSystem.Play();
    }
}
