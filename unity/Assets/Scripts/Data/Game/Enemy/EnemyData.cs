using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData {
    
    [SerializeField] private string name;
    [SerializeField] private RaceID raceId;
    [SerializeField] private AiParameters aiParams;
    [SerializeField] private List<ItemData> items;

    public EnemyData(EnemyData other)
    {
        name = other.name;
        raceId = new RaceID(other.raceId);
        aiParams = new AiParameters(other.aiParams);
        items = new List<ItemData>(other.items);
    }

    public string Name
    {
        get { return name; }
    }

    public RaceID RaceId
    {
        get { return raceId; }
    }

    public AiParameters AiParams
    {
        get { return aiParams; }
    }

    public List<ItemData> Items
    {
        get {
            if (items == null) items = new List<ItemData>();
            return items;
        }
    }
	
}
