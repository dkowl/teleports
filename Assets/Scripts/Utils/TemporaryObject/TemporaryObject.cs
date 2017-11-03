using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TemporaryObject {

    public enum EffectType
    {
        DelayedDestruction,
        Floating
    }

    [SerializeField] GameObject prefab;
    [SerializeField] Transform spawnTransform;
    [SerializeField] EffectType[] effects;

    [SerializeField] float delayedDestructionTime;

    [SerializeField] Vector3 floatingGravity;
    [SerializeField] Vector3 floatingStartVelocity;

    List<GameObject> spawnedObjects;

    public void Spawn()
    {
        if (spawnedObjects == null)
            spawnedObjects = new List<GameObject>();

        GameObject spawnedObject = Object.Instantiate(prefab, spawnTransform) as GameObject;
        spawnedObjects.Add(spawnedObject);
        foreach(var effect in effects)
        {
            AddEffect(spawnedObject, effect);
        }
    }

    void AddEffect(GameObject spawnedObject, EffectType effectType)
    {
        switch (effectType)
        {
            case EffectType.DelayedDestruction:
                DelayedDestructionEffect tdod = spawnedObject.AddComponent<DelayedDestructionEffect>();
                tdod.DelayTime = delayedDestructionTime;
                break;
            case EffectType.Floating:
                FloatingEffect fl = spawnedObject.AddComponent<FloatingEffect>();
                fl.Gravity = floatingGravity;
                fl.StartVelocity = floatingStartVelocity;
                break;
        }
    }




}
