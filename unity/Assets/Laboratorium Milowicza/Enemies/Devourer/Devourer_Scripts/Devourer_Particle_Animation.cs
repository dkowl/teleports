using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devourer_Particle_Animation : MonoBehaviour {


    public ParticleSystem GroundEruptParticleSystem;
    public ParticleSystem GroundDigParticleSystem;

    public ParticleSystem SideFlopParticleSystem;
    public ParticleSystem SideHeavyFlopParticleSystem;
    public ParticleSystem SideTailSmackParticleSystem;

    public ParticleSystem TailStrikeParticleSystem;

    public ParticleSystem IndexFingerParticleSystem;
    public ParticleSystem MiddleFingerParticleSystem;
    public ParticleSystem PinkyFingerParticleSystem;
    public ParticleSystem TailTrailParticleSystem;

    public ParticleSystem LeftHandParticleSystem;
    public ParticleSystem RightHandParticleSystem;
    public ParticleSystem WalkTrailParticleSystem;

    void GroundErupt ()
    {
        GroundEruptParticleSystem.Play();
	}

    void GroundDig()
    {
        GroundDigParticleSystem.Play();
    }
    void StopEmission()
    {
        var emmit = GroundDigParticleSystem.emission;
        emmit.enabled = false;
        StartCoroutine(Wait());
        GroundDigParticleSystem.Stop();
        emmit.enabled = true;
    }
    IEnumerator Wait() {
        yield return new WaitForSeconds(10);
    }
    void SideFlop() {
        SideFlopParticleSystem.Play();
    }
    void SideHeavyFlop() {
        SideHeavyFlopParticleSystem.Play();
    }
    void TailSmack() {
        SideTailSmackParticleSystem.Play();
    }
    void TailStrike() {
        TailStrikeParticleSystem.Play();
    }
    void Swipe() {
        IndexFingerParticleSystem.Play();
        MiddleFingerParticleSystem.Play();
        PinkyFingerParticleSystem.Play();
    }
    void TailTrail() {
        TailTrailParticleSystem.Play();
    }
    void WalkTrailStart() {
        WalkTrailParticleSystem.Play();
        var WalkEmision = WalkTrailParticleSystem.emission;
        WalkEmision.enabled = true;

    }
    void WalkTrailStop() {
        var WalkEmision = WalkTrailParticleSystem.emission;
        WalkEmision.enabled = false;
    }
    void LeftHandWalk() {
        LeftHandParticleSystem.Play();
    }
    void RightHandWalk() {
        RightHandParticleSystem.Play();
    }
}
