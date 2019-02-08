using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIData
{
    [SerializeField] private GameObject mainCanvasPrefab;
    [SerializeField] private GameObject popupMenuPrefab;
    [SerializeField] private StylesheetSO stylesheet;
    [SerializeField] private Stylesheet_Legacy stylesheetLegacy;
    [SerializeField] private MappedList<MenuData> menus;

    public GameObject MainCanvasPrefab => mainCanvasPrefab;
    public GameObject PopupMenuPrefab => popupMenuPrefab;
    public IStylesheet Stylesheet => stylesheet.Data;
    public Stylesheet_Legacy StylesheetLegacy => stylesheetLegacy;

    public IMappedList<MenuData> Menus => menus;
    public MappedList<MenuData> MenusConcrete => menus;
}
