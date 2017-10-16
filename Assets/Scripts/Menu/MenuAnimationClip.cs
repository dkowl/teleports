using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MenuAnimationClip {
    
    [SerializeField] public AnimationClip clip;
    [SerializeField] public MenuBehaviour.State state;

}
