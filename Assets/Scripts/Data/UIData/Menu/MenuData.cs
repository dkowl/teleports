using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class MenuData : UniquelyNamedObject {

	[SerializeField] private GameObject prefab;
    [SerializeField] private bool disableMenusUnder;
    [Tooltip("will parent the menu to the MainCanvas prefab")]
    [SerializeField] private bool useMainCanvas;
    [Tooltip("higher values will appear on top")]
    [SerializeField] private int sortOrder;

    public GameObject Prefab => prefab;
    public bool DisableMenusUnder => disableMenusUnder;
    public bool UseMainCanvas => useMainCanvas;
    public int SortOrder => sortOrder;
}