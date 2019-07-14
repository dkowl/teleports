using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceModelSpawner : ModelSpawner {

    [SerializeField] CreateCharacterMenu racePicker;

    protected override GameObject GetModel(int id)
    {
        RaceGraphics raceGraphics = racePicker.Race.Graphics;
        GameObject result = Instantiate(raceGraphics.ModelObject, transform);
        result.transform.localEulerAngles = new Vector3(0, 180, 0);
        result.GetComponentInChildren<Animator>().runtimeAnimatorController = raceGraphics.UiAnimationController;

        return result;
    }

    protected override void SubscribeInternal()
    {
        racePicker.RaceIdChangedEvent += ShouldRespawn;
    }

    protected override void UnsubscribeInternal()
    {
        racePicker.RaceIdChangedEvent -= ShouldRespawn;
    }
}
