using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy/AssetData")]
public class EnemyAssetData : UniqueScriptableObject {
    
    [SerializeField] private EnemyData baseEnemyData;
    [SerializeField] private List<ItemID> itemIds;

    public EnemyData GenerateBasic()
    {
        EnemyData result = new EnemyData(baseEnemyData);
        foreach(var itemId in itemIds)
        {
            result.Items.Add(MainData.Game.GetItem(itemId));
        }
        return result;
    }

}
