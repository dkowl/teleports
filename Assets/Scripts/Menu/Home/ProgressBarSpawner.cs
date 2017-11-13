using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarSpawner : PrefabSpawner {

    protected override void AfterSpawn()
    {
        GetComponent<BaseProgressBarUI>().ChildrenSetup();
    }
}
