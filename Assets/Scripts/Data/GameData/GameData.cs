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
    [SerializeField] private MappedListOfItems items;
    [SerializeField] private SkillDatabase skillDatabase;

    public void OnEnable()
    {
        gems.MakeDict();
        worlds.MakeDict();
        races.MakeDict();
        skills.MakeDict();
        items.MakeDict();
    }

    public ItemData GetItem(string itemName)
    {
        ItemData result = items.TryGetValue(itemName);

        if (result != null)
        {
            return result;
        }
        else
        {
            Debug.Log("Item " + itemName + " not found.");
            return null;
        }
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
