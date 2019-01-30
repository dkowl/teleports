using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Assertions;
using Teleports.Utils;

public class GameState : IGameState
{
    private const int StartingHeroSlotCount = 3;

    [SerializeField] private string accountName;
    [SerializeField] private List<HeroData> heroes;
    [SerializeField] private int currentHeroId;

    public static event System.Action HeroChangedEvent;
    public static event System.Action GameStateUpdatedEvent; 

    public GameState(string accountName)
    {
        this.accountName = accountName;
        currentHeroId = 0;
        Utils.InitWithValues(ref heroes, StartingHeroSlotCount, null);
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

    public void Update(IGameSessionResult sessionResult)
    {
        List<PostGamePopUpEvent> postGamePopUpEvents;
        CurrentHeroData.AddXp(sessionResult.XpEarned, out postGamePopUpEvents);
        Main.UISystem.HandlePostGamePopUpEvents(postGamePopUpEvents);
        Main.MessageBus.Publish(new RunFinishedMessage(sessionResult.XpEarned));
        GameStateUpdatedEvent?.Invoke();
    }

    public HeroData GetHeroData(int id)
    {
        AssertValidHeroId(id);
        return heroes[id];
    }

    public HeroData CurrentHeroData => GetHeroData(currentHeroId);
    public int CurrentHeroId => currentHeroId;
    public int HeroSlotCount => heroes.Count;

    private void AssertValidHeroId(int id)
    {
        Debug.Assert(id >= 0 && id < heroes.Count, "Invalid hero id");
    }

    public void CorrectInvalidData()
    {
        foreach (var hero in heroes)
        {
            hero?.CorrectInvalidData();
        }
    }
}
