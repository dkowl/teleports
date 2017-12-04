using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class ItemAssetData : UniqueScriptableObject {

    [SerializeField] private ItemData baseItemData;

    public ItemData GenerateItem()
    {
        return new ItemData(baseItemData);
    }
}
