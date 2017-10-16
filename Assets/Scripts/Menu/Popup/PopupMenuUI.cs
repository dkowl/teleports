using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenuUI : MenuBehaviour {

    public MenuController.MenuType menuType;

    public override void OnOpen()
    {
        GetComponent<Animation>().Play("OnOpen");
    }

    public void Close()
    {
        MenuController.Instance.CloseMenu(menuType);
    }
}
