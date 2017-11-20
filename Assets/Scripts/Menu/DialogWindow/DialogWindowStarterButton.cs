using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DialogWindowStarterButton : MonoBehaviour {

    [SerializeField] protected string textString;
    protected DialogWindow window;

    void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(Click);
    }

    void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(Click);
    }

    protected abstract List<ButtonChoice> Choices();
    protected virtual string TextString()
    {
        return textString;
    }
    protected virtual bool IsActive()
    {
        return true;
    }

    public void Click()
    {
        if (!IsActive()) return;
        MenuController.Instance.OpenMenu(MenuController.MenuType.DialogWindow);
        Menu windowMenu = MenuController.Instance.GetMenu(MenuController.MenuType.DialogWindow);
        if (windowMenu.InstantiatedObject != null)
        {
            window = windowMenu.InstantiatedObject.GetComponent<DialogWindow>();
            if(window == null) window = windowMenu.InstantiatedObject.GetComponentInChildren<DialogWindow>();

            window.Setup(TextString(), Choices());
        }
    }
}
