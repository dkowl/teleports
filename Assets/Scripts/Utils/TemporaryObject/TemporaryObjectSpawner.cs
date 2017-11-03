using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryObjectSpawner : MonoBehaviour
{

    [SerializeField] TemporaryObject[] objects;

    public void Spawn(int objectId)
    {
        objects[objectId].Spawn();
    }
}
