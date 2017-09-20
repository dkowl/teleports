using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorUI : MonoBehaviour {

    new Animation animation;

    public void Awake()
    {
        animation = GetComponent<Animation>();
    }

    public void Hide()
    {
        animation.Play("SelectorDown");
    }
}
