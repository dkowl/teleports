using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSlotUISpawner : PrefabSpawner {

    [SerializeField]
    private int gemSlotID;

    protected override void AfterSpawn()
    {
        GemSlotUI slot = SpawnedInstance.GetComponent<GemSlotUI>();

        slot.SetSlotID(gemSlotID);
        slot.LoadData();
    }
}
