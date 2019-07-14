using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataDefaults {

	public static readonly string raceName = "Human";
    public static readonly string unitName = "New Unit";
    public static readonly string skillName = "Attack_Default";
    public static readonly string itemName = "Default Item";
    [SerializeField] public ItemID itemId;
    [SerializeField] public ItemGraphicsID itemGraphicsId;
}
