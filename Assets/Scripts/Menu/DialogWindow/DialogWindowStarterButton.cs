using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindowStarterButton : MonoBehaviour {

    [SerializeField] protected string textString;
    protected DialogWindow window;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Click);
    }

	public void Callback()
    {
        MenuController.Instance.CloseTopMenu();
    }

    public void Click()
    {
        MenuController.Instance.OpenMenu(MenuController.MenuType.DialogWindow);
        Menu windowMenu = MenuController.Instance.GetMenu(MenuController.MenuType.DialogWindow);
        if (windowMenu.InstantiatedObject != null)
        {
            window = windowMenu.InstantiatedObject.GetComponent<DialogWindow>();
            if(window == null) window = windowMenu.InstantiatedObject.GetComponentInChildren<DialogWindow>();

            List<ButtonChoice> choices = new List<ButtonChoice>();
            choices.Add(new ButtonChoice("Yes", Callback));
            choices.Add(new ButtonChoice("No", Callback));

            window.Setup(textString, choices);
        }
        else
        {
        }
    }
}
