using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "skillGraphics", menuName = "Data/Skill/Graphics")]
public class SkillGraphics : ScriptableObject {

    [FormerlySerializedAs("uiIcon_")]
    [SerializeField] private Sprite uiIcon;
    [SerializeField] private AnimationClip castAnimation;

    public Sprite UiIcon => uiIcon;
    public AnimationClip CastAnimation => castAnimation;
}
