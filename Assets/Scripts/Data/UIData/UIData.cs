using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIData
{
    [SerializeField] private GameObject mainCanvasPrefab;
    [SerializeField] private Stylesheet stylesheet;

    public GameObject MainCanvasPrefab => mainCanvasPrefab;
    public Stylesheet Stylesheet => stylesheet;
}
