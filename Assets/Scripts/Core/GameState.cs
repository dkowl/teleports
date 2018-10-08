
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class GameState : IGameState
{
    private const int StartingHeroSlotCount = 3;

    private HeroData[] heroes;
    private int currentHeroId;
    private int heroSlotCount;

    public static event System.Action HeroChangedEvent;

    public GameState()
    {
        currentHeroId = 0;
        heroSlotCount = StartingHeroSlotCount;
        heroes = new HeroData[heroSlotCount];
    }

    public void CreateNewHero(string name, RaceID raceId)
    {
        if (CurrentHeroData != null && !CurrentHeroData.IsEmpty)
        {
            ErrorHandler.ReportError("Cannot create a new hero - slot already taken");
            return;
        }

        if (Main.StaticData.Game.Races.GetValue(raceId) == null)
        {
            ErrorHandler.ReportError("Cannot create a new hero - invalid race ID");
            return;
        }

        heroes[currentHeroId] = new HeroData(name, raceId);
        HeroChangedEvent?.Invoke();
    }

    public void DeleteCurrentHero()
    {
        heroes[currentHeroId] = null;
        HeroChangedEvent?.Invoke();
    }

    public void SelectHero(int id)
    {
        AssertValidHeroId(id);
        currentHeroId = id;
        HeroChangedEvent?.Invoke();
    }

    public HeroData GetHeroData(int id)
    {
        AssertValidHeroId(id);
        return heroes[id];
    }

    public HeroData CurrentHeroData => GetHeroData(currentHeroId);
    public int CurrentHeroId => currentHeroId;
    public int HeroSlotCount => heroSlotCount;

    private void AssertValidHeroId(int id)
    {
        Debug.Assert(id >= 0 && id < heroSlotCount, "Invalid hero id");
    }
}
