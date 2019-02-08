using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIData
{
    [SerializeField] private GameObject mainCanvasPrefab;
    [SerializeField] private GameObject popupMenuPrefab;
    [SerializeField] private Stylesheet stylesheet;
    [SerializeField] private MappedList<MenuData> menus;

    public GameObject MainCanvasPrefab => mainCanvasPrefab;
    public GameObject PopupMenuPrefab => popupMenuPrefab;
    public Stylesheet Stylesheet => stylesheet;

    public IMappedList<MenuData> Menus => menus;
    public MappedList<MenuData> MenusConcrete => menus;
}
