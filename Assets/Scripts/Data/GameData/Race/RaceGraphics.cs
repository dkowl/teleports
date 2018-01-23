using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "raceGraphics", menuName = "Data/Race/Graphics")]
public class RaceGraphics : UniqueScriptableObject {

    [SerializeField] private GameObject modelObject;

    [SerializeField] private RuntimeAnimatorController uiAnimationController;
    [SerializeField] private RuntimeAnimatorController worldAnimationController;
    [SerializeField] private Sprite icon; 

    public GameObject ModelObject
    {
        get { return modelObject; }
    }

    public RuntimeAnimatorController UiAnimationController
    {
        get { return uiAnimationController; }
    }

    public RuntimeAnimatorController WorldAnimationController
    {
        get { return worldAnimationController; }
    }

    public Sprite Icon
    {
        get { return icon; }
    }

}
