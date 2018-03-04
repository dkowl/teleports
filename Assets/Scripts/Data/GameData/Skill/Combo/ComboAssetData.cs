using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill/Combo/AssetData")]
public class ComboAssetData : UniqueScriptableObject
{
    [SerializeField]
    private ComboData data;

    public ComboData Data => data;
}
