using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystemStarter : MonoBehaviour {

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        MenuController.Instance.FirstStart(transform);
    }
}
