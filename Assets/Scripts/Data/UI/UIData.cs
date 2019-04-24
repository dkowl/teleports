using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIData
{
    [SerializeField] private GameObject mainCanvasPrefab;
    [SerializeField] private GameObject popupMenuPrefab;
    [SerializeField] private StylesheetSO stylesheet;
    [SerializeField] private MappedList<MenuData> menus;
    [SerializeField] private HUDData hudData;

    public GameObject MainCanvasPrefab => mainCanvasPrefab;
    public GameObject PopupMenuPrefab => popupMenuPrefab;
    public IStylesheet Stylesheet => stylesheet.Data;
    public HUDData HUD => hudData;

    public IMappedList<MenuData> Menus => menus;
    public MappedList<MenuData> MenusConcrete => menus;
}
