using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Menu", menuName = "Menu/Menu")]
public class Menu : ScriptableObject
{
    //private variables
    [System.NonSerialized]
    private GameObject instantiatedObject = null;
    [System.NonSerialized]
    private bool isOpen = false, isActive = false;

    //inspector variables
    public GameObject prefab;
    public MenuController.MenuType menuType;
    public bool disableMenusUnder; //menu will be closed when enabled back
    [Tooltip("will parent the menu to the MainCanvas prefab")]
    public bool useMainCanvas;

    //public functions
    public void Open()
    {
        if (!IsOpen)
        {
            if (instantiatedObject == null)
            {
                Debug.Log("Opening Menu");
                if (useMainCanvas)
                {
                    instantiatedObject = Instantiate(MenuController.MainCanvasPrefab, MenuController.SpawnTransform) as GameObject;
                    Instantiate(prefab, instantiatedObject.transform);
                }
                else
                {
                    instantiatedObject = Instantiate(prefab, MenuController.SpawnTransform) as GameObject;
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
