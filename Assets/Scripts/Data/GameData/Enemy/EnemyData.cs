using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData {

    [SerializeField] private RaceID raceId;
    [SerializeField] private AiParameters aiParams;

    public EnemyData(EnemyData other)
    {
        raceId = other.raceId;
    }
	
}
