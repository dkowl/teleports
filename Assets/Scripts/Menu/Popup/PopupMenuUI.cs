using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenuUI : MenuBehaviour {

    new private Animation animation;

    public MenuController.MenuType menuType;

    void Awake()
    {
        animation = GetComponent<Animation>();
    }

    public override void OnOpen()
    {
        animation.Play("OnOpen");
    }

    public void Close()
    {
        MenuController.Instance.CloseMenu(menuType);
    }
}
