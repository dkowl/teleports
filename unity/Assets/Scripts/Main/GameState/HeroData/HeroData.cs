using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Teleports.Utils;

public partial class HeroData
{
    private const int StartingSkillSlotsCount = 3;
    private const int StartingSecondarySkillsCount = 2;
    public const int AttributePointsPerLevel = 3;

    public HeroData(string name, RaceID raceId)
    {
        Init(name, raceId);
    }

    public void Init(string name, RaceID raceId)
    {
        isEmpty = false;
        characterName = name;
        this.raceId = raceId;
        xp = 0;
        level = 1;
        rankPoints = 0;
        Utils.InitWithNew(ref skillTreeSlots, StartingSkillSlotsCount);

        unitData = Main.StaticData.Game.Races.GetValue(raceId).BaseStats;
        skills = unitData.Skills;
        primarySkill = unitData.MainAttack;
        secondarySkills = new List<SkillID>(StartingSecondarySkillsCount);

        teleportData = new TeleportData();
    }

    public float GetStat(PlayerStats type)
    {
        switch (type)
        {
            case PlayerStats.Hp:
                return UnitData.GetAttribute(UnitAttributesData.AttributeType.HealthPoints).Value;
            case PlayerStats.Armor:
                return UnitData.GetAttribute(UnitAttributesData.AttributeType.Armor).Value;
            case PlayerStats.ArmorIgnore:
                return 0; //TODO
            case PlayerStats.Damage:
                return 0; //TODO
            case PlayerStats.MoveSpeed:
                return UnitData.GetAttribute(UnitAttributesData.AttributeType.MovementSpeed).Value;
            case PlayerStats.Reach:
                return UnitData.GetAttribute(UnitAttributesData.AttributeType.Reach).Value;
            case PlayerStats.Regen:
                return UnitData.GetAttribute(UnitAttributesData.AttributeType.Regeneration).Value;
            case PlayerStats.ViewRange:
                return UnitData.GetAttribute(UnitAttributesData.AttributeType.ViewRange).Value;
            case PlayerStats.DamagePerSecond:
                return 0; //TODO: Divide by attack speed
            case PlayerStats.TeleportPower:
                return TeleportData.Power;
            case PlayerStats.TeleportTime:
                return TeleportData.Time;
            default:
                return 0.0f;
        }
    }

    public void AddXp(int xpToAdd, out List<PostGamePopUpEvent> postGamePopUpEvents)
    {
        int oldXp = xp;
        int oldRp = rankPoints;
        xp = xp + xpToAdd;
        UpdateLevel();
        UpdateRankPoints(xpToAdd);
        int newXp = xp;
        int newRp = rankPoints;
        postGamePopUpEvents = GeneratePostGamePopUpEvents(oldXp, newXp, oldRp, newRp);
    }

    public enum ApplyAttributePointsResult
    {
        OK,
        WrongAttributeType,
        NotEnoughPoints
    }

    public ApplyAttributePointsResult ApplyAttributePoints(List<System.Tuple<UnitAttributesData.AttributeType, int>> pointsToApply)
    {
        int totalPointsToApply = 0;
        foreach(var pts in pointsToApply)
        {
            if (!IsValidAttributeTypeToUpgrade(pts.Item1)) return ApplyAttributePointsResult.WrongAttributeType;
            totalPointsToApply += pts.Item2;
        }
        if (totalPointsToApply > TotalAttributePoints) return ApplyAttributePointsResult.NotEnoughPoints;
        foreach(var pts in pointsToApply)
        {
            UnitData.Attributes.GetAttribute(pts.Item1).AddBase(pts.Item2);
        }
        totalAttributePoints -= totalPointsToApply;
        return ApplyAttributePointsResult.OK;
    }

    private bool IsValidAttributeTypeToUpgrade(UnitAttributesData.AttributeType type)
    {
        return 
            type == UnitAttributesData.AttributeType.Strength ||
            type == UnitAttributesData.AttributeType.Dexterity ||
            type == UnitAttributesData.AttributeType.Intelligence;
    }

    private void UpdateLevel()
    {
        int currentLevel = level;
        int newLevel = Levels.xp.Level(xp);

        for (int i = currentLevel + 1; i <= newLevel; i++)
        {
            LevelUp(i);
        }
    }

    private void LevelUp(int newLevel)
    {
        level = newLevel;

        totalAttributePoints += AttributePointsPerLevel;
    }
    
    private void UpdateRankPoints(int score)
    {
        rankPoints = RankPointUpdater.UpdateRankPoints(rankPoints, score);
    }

    private static List<PostGamePopUpEvent> GeneratePostGamePopUpEvents(int oldXp, int newXp, int oldRp, int newRp)
    {
        var result = new List<PostGamePopUpEvent>();
        var xpIntervals = Levels.xp.GetSliderProgressionIntervals(oldXp, newXp);
        var rpIntervals = Levels.rp.GetSliderProgressionIntervals(oldRp, newRp);
        for (int i = 0; i < xpIntervals.Count; i++)
        {
            var data = new PostGamePopUpEvent_XpEarned.Data
            {
                isStartingASequence = i == 0,
                isEndingASequence = i == xpIntervals.Count - 1,
                oldXp = xpIntervals[i].Item1,
                newXp = xpIntervals[i].Item2
            };
            result.Add(new PostGamePopUpEvent_XpEarned(data));
            if (i >= xpIntervals.Count - 1) continue;

            int newLevel = Levels.xp.Level(data.newXp);
            result.Add(new PostGamePopUpEvent_LevelUp(newLevel, AttributePointsPerLevel));
        }

        for (int i = 0; i < rpIntervals.Count; i++)
        {
            var data = new PostGamePopUpEvent_RpEarned.Data()
            {
                isStartingASequence = i == 0,
                isEndingASequence = i == rpIntervals.Count - 1,
                oldRp = rpIntervals[i].Item1,
                newRp = rpIntervals[i].Item2
            };
            result.Add(new PostGamePopUpEvent_RpEarned(data));
            if (i >= rpIntervals.Count - 1) continue;

            int oldRank = Levels.rp.Level(data.oldRp);
            int newRank = Levels.rp.Level(data.newRp);
            result.Add(new PostGamePopUpEvent_RankChange(oldRank, newRank));
        }

        return result;
    }

    public void CorrectInvalidData()
    {
        if (raceId == null)
        {
            Debug.LogWarning("Race ID is null, changing to \"Human\"");
            raceId = new RaceID("Human");
        }
    }
}
