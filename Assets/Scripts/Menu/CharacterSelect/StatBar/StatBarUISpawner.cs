using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBarUISpawner : PrefabSpawner {

    public PlayerStats statType;
    public string statName;

    public override void AfterSpawn()
    {
        StatBarUI statBar = spawnedInstance.GetComponent<StatBarUI>();

        statBar.statType = statType;
        statBar.statName = statName;
        statBar.LoadData();
    }
}
