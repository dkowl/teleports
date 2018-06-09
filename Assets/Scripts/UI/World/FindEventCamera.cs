using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEventCamera : MonoBehaviour {

    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = GameplayMain.Instance.uiCamera;
    }
}
