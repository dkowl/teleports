using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
[CreateAssetMenu(fileName = "gameData", menuName = "Data/Game")]
public class GameData : ScriptableObject {

    [SerializeField] private MappedListOfGems gems;
    [SerializeField] private MappedListOfWorlds worlds;
    [SerializeField] private MappedListOfRaces races;
    [SerializeField] private MappedListOfSkills skills;
    [SerializeField] private SkillDatabase skillDatabase;
    [SerializeField] private EquipmentSlotDatabase equipmentSlotDatabase;

    public void OnEnable()
    {
        gems.MakeDict();
        worlds.MakeDict();
        races.MakeDict();
        skills.MakeDict();
        equipmentSlotDatabase.Initialize();
    }
    
    public Race GetRace(string raceName)
    {
        Race result = races.TryGetValue(raceName);

        if (result != null)
        {
            return result;
        }
        else
        {
            Debug.Log("Race " + raceName + " not found.");
            return null;
        }
    }

    public Skill GetSkill(SkillID skillId)
    {
        if (skillId.UsesString())
        {
            return skills.TryGetValue(skillId.skillName);
        }
        else
        {
            return skillDatabase.GetSkill(skillId);
        }
    }

    public SkillDatabase CurrentSkillDatabase
    {
        get { return skillDatabase; }
    }
}
