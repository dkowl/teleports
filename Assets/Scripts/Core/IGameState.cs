using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    void CreateNewHero(string name, RaceID raceId);
    void DeleteCurrentHero();
    void SelectHero(int heroSlotId);
    HeroData CurrentHeroData { get; }
    HeroData GetHeroData(int heroSlotId);
    int CurrentHeroId { get; }
    int HeroSlotCount { get; }
}
