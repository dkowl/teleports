using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill/AssetData")]
public class SkillAssetData : UniqueScriptableObject{

    [SerializeField] private SkillData data;

    public SkillData Data
    {
        get { return data; }
    }
}
