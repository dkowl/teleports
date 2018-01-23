using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy/AssetData")]
public class EnemyAssetData : UniqueScriptableObject {
    
    [SerializeField] private EnemyData baseEnemyData;

    public EnemyData GenerateBasic()
    {
        return new EnemyData(baseEnemyData);
    }

}
