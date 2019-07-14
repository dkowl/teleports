using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotUISpawner : PrefabSpawner
{

    [SerializeField]
    private int skillSlotID;

    protected override void AfterSpawn()
    {
        SkillSlotUI slot = SpawnedInstance.GetComponent<SkillSlotUI>();

        slot.SetSlotID(skillSlotID);
        slot.LoadData();
    }
}
