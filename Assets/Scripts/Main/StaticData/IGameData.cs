using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameData  {

    IMappedList<Gem> Gems { get; }
    IMappedList<WorldData> Worlds { get; }
    IMappedList<Race> Races { get; }
    IMappedList<Perk> Perks { get; }
    IMappedList<SkillAssetData> Skills { get; }
    IMappedList<ComboAssetData> Combos { get; }
    IMappedList<ItemAssetData> Items { get; }
    IMappedList<EnemyAssetData> Enemies { get; }

    List<Race> GetPlayableRaces();
}
