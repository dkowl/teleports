using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Data/Item/AssetData")]
public class ItemAssetData : UniqueScriptableObject {

    [SerializeField] private ItemData baseItemData;

    public ItemData GenerateItem()
    {
        //Debug.Log("Generating Item: " + baseItemData.DisplayName);
        return new ItemData(baseItemData, baseItemData.DisplayName);
    }
}
