using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "raceGraphics", menuName = "Data/Race/Graphics")]
public class RaceGraphics : ScriptableObject {

    public GameObject modelObject;
    
    public RuntimeAnimatorController uiAnimationController;
    public Sprite icon; 
}
