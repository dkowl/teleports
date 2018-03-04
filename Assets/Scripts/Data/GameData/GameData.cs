using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
[CreateAssetMenu(fileName = "gameData", menuName = "Data/Game")]
[ShowOdinSerializedPropertiesInInspector]
public class GameData : SerializedScriptableObject {

    [SerializeField] private MappedList<Gem> gems;
    [SerializeField] private MappedList<WorldData> worlds;
    [SerializeField] private MappedList<Race> races;
    [SerializeField] private MappedList<Perk> perks;
    [SerializeField] private MappedList<SkillAssetData> skills;
    [SerializeField] private MappedList<ComboAssetData> combos;
    [SerializeField] private MappedList<ItemAssetData> items;
    [SerializeField] private MappedList<EnemyAssetData> enemies;
    [SerializeField] private GraphicsData graphicsData;

    public void OnEnable()
    {
        gems.MakeDict();
        worlds.MakeDict();
        races.MakeDict();
        perks.MakeDict();
        skills.MakeDict();
        combos.MakeDict();
        items.MakeDict();
        enemies.MakeDict();
    }

    public ItemData GetItem(string itemName)
    {
        ItemAssetData result = items.TryGetValue(itemName);

        if (result != null)
        {
            return result.GenerateItem();
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

    public Race GetRace(RaceID raceId)
    {
        return GetRace(raceId.Name);
    }

    public List<Race> GetPlayableRaces()
    {
        var result = new List<Race>();

        foreach (Race race in races.AllValues)
        {
            if (race.IsPlayable)
            {
                result.Add(race);
            }
        }

        return result;
    }

    public SkillData GetSkill(SkillID skillId)
    {
        return skillId.UsesString() ? skills.TryGetValue(skillId.Name).Data : null;
    }

    public Perk GetPerk(PerkID perkId)
    {
        return perks.TryGetValue(perkId.Name);
    }

    public EnemyData GetEnemy(EnemyID enemyId)
    {
        EnemyAssetData result = enemies.TryGetValue(enemyId.Name);
        return result != null ? result.GenerateBasic() : null;
    }

    public IMappedList<Gem> Gems => gems;
    public IMappedList<WorldData> Worlds => worlds;
    public IMappedList<Race> Races => races;
    public IMappedList<Perk> Perks => perks;
    public IMappedList<SkillAssetData> Skills => skills;
    public IMappedList<ComboAssetData> Combos => combos;
    public IMappedList<ItemAssetData> Items => items;
    public IMappedList<EnemyAssetData> Enemies => enemies;
    public GraphicsData GraphicsData => graphicsData;
    public IList<string> PerkNames => perks.AllNames;
    public IList<string> SkillNames => skills.AllNames;
    public IList<string> ItemNames => items.AllNames;
    public IList<string> EnemyNames => enemies.AllNames;
    public IList<string> RaceNames => races.AllNames;
}
