using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSlotUISpawner : PrefabSpawner {

    [SerializeField]
    private int gemSlotID;

    public override void AfterSpawn()
    {
        GemSlotUI slot = spawnedInstance.GetComponent<GemSlotUI>();

        slot.SetSlotID(gemSlotID);
        slot.LoadData();
    }
}
