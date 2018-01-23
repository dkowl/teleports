using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData {

    [SerializeField] private string graphicsId;
    [SerializeField] private UnitData stats;

    public EnemyData(EnemyData other)
    {
        graphicsId = other.graphicsId;
        stats = other.stats;
    }
	
}
