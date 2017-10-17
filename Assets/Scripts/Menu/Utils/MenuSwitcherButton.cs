using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuSwitcherButton : MonoBehaviour {

    [SerializeField]
    protected MenuController.MenuType menuType;
    protected Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        MenuController.Instance.OpenMenu(menuType);
    }
}
