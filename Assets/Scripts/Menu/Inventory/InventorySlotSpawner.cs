using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotSpawner : PrefabSpawner {

    Texture2D atlas;
    Rect[] uvs;

    protected override void AfterSpawn()
    {
        RawImage rawImage = SpawnedInstance.GetComponentInChildren<RawImage>();

        if (rawImage != null)
        {
            if (atlas != null && uvs != null && currentId < uvs.Length)
            {
                rawImage.texture = atlas;
                rawImage.uvRect = uvs[currentId];
            }
            else
            {
                rawImage.color = Color.clear;
            }
        }
    }

    public Texture2D Atlas
    {
        set { atlas = value; }
    }

    public Rect[] Uvs
    {
        set { uvs = value; }
    }
}
