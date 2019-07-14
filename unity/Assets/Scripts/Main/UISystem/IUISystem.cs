using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CanvasSortOrder
{
    Normal = 0,
    CheatConsole = 10,
    LoadingScreen = 20
}

public interface IUISystem
{

    void InitInEditMode();
    void Start();
    GameObject SpawnCanvas(string name, CanvasSortOrder sortOrder = CanvasSortOrder.Normal);
    GameObject SpawnPrefab(GameObject prefab);
    GameObject SpawnMenu(MenuData menuData);
    void HandlePostGamePopUpEvents(IEnumerable<PostGamePopUpEvent> popUpEvents);

    IMenuController MenuController { get; }
}
