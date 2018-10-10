using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystemStarter : MonoBehaviour
{
    private static bool started;

    public void Start()
    {
        if (started) return;

        DontDestroyOnLoad(gameObject);
        MenuController.Instance.FirstStart(transform);
        started = true;
    }
}
