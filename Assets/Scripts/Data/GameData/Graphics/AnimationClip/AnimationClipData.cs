using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Graphics/AnimationClip")]
public class AnimationClipData : UniqueScriptableObject
{
    [SerializeField] private AnimationClip clip;
    [SerializeField] private RaceGraphicsID raceGraphicsId;

    public AnimationClip Clip => clip;
    public RaceGraphicsID RaceGraphicsId => raceGraphicsId;
}
