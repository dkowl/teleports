using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "raceGraphics", menuName = "Data/Race/Graphics")]
public class RaceGraphics : UniqueScriptableObject {

    [SerializeField] private GameObject modelObject;
    [SerializeField] private GameObject ragdollObject;
    [SerializeField] private Collider collider;

    [SerializeField] private RuntimeAnimatorController uiAnimationController;
    [SerializeField] private RuntimeAnimatorController worldAnimationController;
    [SerializeField] private Sprite icon; 

    public GameObject ModelObject => modelObject;
    public GameObject RagdollObject => ragdollObject;
    public Collider Collider => collider;
    public RuntimeAnimatorController UiAnimationController => uiAnimationController;
    public RuntimeAnimatorController WorldAnimationController => worldAnimationController;
    public Sprite Icon => icon;
}
