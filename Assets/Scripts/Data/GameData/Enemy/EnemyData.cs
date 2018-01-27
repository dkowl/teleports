using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData {
    
    [SerializeField] private string name;
    [SerializeField] private RaceID raceId;
    [SerializeField] private AiParameters aiParams;

    public EnemyData(EnemyData other)
    {
        raceId = other.raceId;
        aiParams = other.aiParams;
        name = other.name;
    }

    public RaceID RaceId
    {
        get { return raceId; }
    }

    public AiParameters AiParams
    {
        get { return aiParams; }
    }

    public string Name
    {
        get { return name; }
    }
	
}
