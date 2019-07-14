using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public static class UniqueID<T> {

    private static int nextID;

    public static int NextID
    {
        get
        {
            return nextID++;
        }
    }
}
