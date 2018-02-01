using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystemStarter : MonoBehaviour {

    private bool started = false;

    public void Start()
    {
        if (!started)
        {
            DontDestroyOnLoad(gameObject);
            MenuController.Instance.FirstStart(transform);
        }
    }
}
