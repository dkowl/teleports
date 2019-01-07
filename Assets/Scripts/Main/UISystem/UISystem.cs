using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : IUISystem
{
    private List<GameObject> canvases;
    private GameObject gameObject;
    private MenuController menuController;

    public void Start()
    {
        Debug.Assert(gameObject == null && canvases == null, "Starting an already started UISystem");

        InitGameObject();
        canvases = new List<GameObject>();
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

    public void HandlePostGamePopUpEvents(IEnumerable<PostGamePopUpEvent> popUpEvents)
    {
        Debug.LogWarning("Not implemented");
    }

    public GameObject GameObject => gameObject;

    public MenuController MenuController
    {
        get
        {
            Debug.Assert(menuController != null);
            return menuController;
        }
    }


    private GameObject MainCanvasPrefab => Main.StaticData.UI.MainCanvasPrefab;
}
