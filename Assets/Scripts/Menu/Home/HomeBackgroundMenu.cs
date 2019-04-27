using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBackgroundMenu : MonoBehaviour {

    [SerializeField] MenuSwitcherButtonUISpawner buttonSpawner;
    
	void OnEnable () {
        MenuController.Instance.OpenMenu(MenuController.MenuIdHomeForeground);
	}
    
    void Update()
    {
        if (!buttonSpawner.HasAnyActive)
        {
            MenuController.Instance.OpenMenu(MenuController.MenuIdHomeForeground);
        }
    }

}
