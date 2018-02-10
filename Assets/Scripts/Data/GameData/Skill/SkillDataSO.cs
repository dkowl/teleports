using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill/AssetData")]
public class SkillAssetData : UniqueScriptableObject{

    public Skill skill; //temporary
    [SerializeField] private SkillData data;

    private void OnValidate()
    {
        data.PopulateFromSkill(skill);
    }

    public SkillData Data
    {
        get { return data; }
    }
}
