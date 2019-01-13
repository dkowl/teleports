using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : IUISystem
{
    private List<GameObject> canvases;
    private GameObject gameObject;
    private IMenuController menuController;

    public UISystem()
    {
        canvases = new List<GameObject>();
    }

    public void InitInEditMode()
    {
        menuController = new MenuControllerStub();
    }

    public void Start()
    {
        Debug.Assert(gameObject == null, "Starting an already started UISystem");
        
        InitGameObject();
        menuController = new MenuController();
        menuController.FirstStart();
    }

    public void InitGameObject()
    {
        gameObject = new GameObject("UI System");
        gameObject.transform.parent = Main.MainGameObject.transform;
    }

    public GameObject SpawnCanvas(string name, CanvasSortOrder sortOrder = CanvasSortOrder.Normal)
    {
        Debug.Assert(gameObject != null && canvases != null);

        GameObject newCanvasObject = Object.Instantiate(MainCanvasPrefab, gameObject.transform);
        newCanvasObject.name = name;

        Canvas newCanvas = newCanvasObject.GetComponent<Canvas>();
        newCanvas.sortingOrder = (int) sortOrder;

        canvases.Add(newCanvasObject);
        return newCanvasObject;
    }

    public GameObject SpawnPrefab(GameObject prefab)
    {
        Debug.Assert(gameObject != null);
        GameObject newObject = Object.Instantiate(prefab);
        newObject.transform.parent = gameObject.transform;
        return newObject;
    }

    public GameObject SpawnMenu(MenuData menuData)
    {
        GameObject containerObject = new GameObject(menuData.UniqueName);
        containerObject.transform.parent = gameObject.transform;

        GameObject canvasObject = Object.Instantiate(MainCanvasPrefab, containerObject.transform);
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.sortingOrder = menuData.SortOrder;

        Debug.Assert(menuData.Prefab != null);
        Object.Instantiate(menuData.Prefab, canvasObject.transform);
        if (menuData.NonCanvasPrefab != null)
        {
            Object.Instantiate(menuData.NonCanvasPrefab, containerObject.transform);
        }

        return containerObject;
    }

    public void HandlePostGamePopUpEvents(IEnumerable<PostGamePopUpEvent> popUpEvents)
    {
        Debug.LogWarning("Not implemented");
    }

    public GameObject GameObject => gameObject;

    public IMenuController MenuController
    {
        get
        {
            Debug.Assert(menuController != null);
            return menuController;
        }
    }


    private GameObject MainCanvasPrefab => Main.StaticData.UI.MainCanvasPrefab;
}
