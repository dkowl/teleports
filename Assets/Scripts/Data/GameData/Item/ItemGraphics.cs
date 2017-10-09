using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New ItemGraphics", menuName = "Data/Item/Graphics")]
public class ItemGraphics : ScriptableObject {

    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite icon;
}
