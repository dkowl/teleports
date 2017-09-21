using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour {

    public void Start()
    {
        MenuController.Instance.FirstStart(transform);
    }
}
