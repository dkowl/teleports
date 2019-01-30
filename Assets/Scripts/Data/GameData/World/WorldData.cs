using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData : UniquelyNamedObject
{

    [SerializeField] private Material terrainMaterial;

    public Material TerrainMaterial => terrainMaterial;

}
