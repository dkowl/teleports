using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    //private variables
    private GameObject instantiatedObject;
    private bool isOpen, isActive;

    //inspector variables
    public string prefabPath;
    public MenuController.MenuType menuType;
    public bool disableMenusUnder; //menu will be closed when enabled back
    [Tooltip("will parent the menu to the MainCanvas prefab")]
    public bool useMainCanvas;

    //public functions
    public void Open()
    {
        if(!IsOpen)
        {
            if (instantiatedObject == null)
            {
                if (useMainCanvas)
                {
                    instantiatedObject = Instantiate(Resources.Load(MenuController.MainCanvasPrefabPath), transform) as GameObject;
                    Instantiate(Resources.Load(prefabPath), instantiatedObject.transform);
                }
                else
                {
                    instantiatedObject = Instantiate(Resources.Load(prefabPath), transform) as GameObject;
                }
            }
            else
            {
                instantiatedObject.SetActive(true);
            }
            IsOpen = true;
        }
    }

    public void Close()
    {
        if (IsOpen && instantiatedObject != null)
        {
            Destroy(instantiatedObject);
            instantiatedObject = null;
            IsOpen = false;
        }
    }

    public void Show()
    {
        if (IsOpen && !isActive && instantiatedObject != null)
        {
            instantiatedObject.SetActive(true);
            isActive = true;
        }
    }

    public void Hide()
    {
        if(IsOpen && isActive && instantiatedObject != null)
        {
            instantiatedObject.SetActive(false);
            isActive = false;
        }
    }

    //properties
    public bool IsOpen
    {
        get { return isOpen; }
        private set
        {
            isOpen = value;
            isActive = value;
        }
    }
    public bool IsActive
    {
        get { return isActive; }
    }
}
