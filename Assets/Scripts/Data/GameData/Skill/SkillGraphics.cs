using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "skillGraphics", menuName = "Custom/Skill/Graphics", order = 3)]
public class SkillGraphics : ScriptableObject {

    [FormerlySerializedAs("uiIcon_")]
    public Sprite uiIcon;

    public AnimationClip castAnimation;
}
