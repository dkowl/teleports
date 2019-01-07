using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenuUI : MenuBehaviour {

    public MenuID menuId;

    protected override void OnOpenInternal()
    {
        GetComponent<Animation>().Play("OnOpen");
        base.OnOpenInternal();
    }

    public void Close()
    {
        MenuController.Instance.CloseMenu(menuId);
    }
}
