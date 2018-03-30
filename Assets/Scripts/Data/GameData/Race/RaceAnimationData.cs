using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceAnimationData {

	[SerializeField] private AnimationClip move;
	[SerializeField] private AnimationClip idle01;
	[SerializeField] private AnimationClip idle02;

    public AnimationClip Move => move;
    public AnimationClip Idle01 => idle02;
    public AnimationClip Idle02 => idle02;
}
