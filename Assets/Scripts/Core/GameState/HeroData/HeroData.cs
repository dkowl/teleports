using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public partial class HeroData
{
    private const int StartingSkillSlotsCount = 3;
    private const int StartingSecondarySkillsCount = 2;

    public HeroData(string name, RaceID raceId)
    {
        Init(name, raceId);
    }

    public void Init(string name, RaceID raceId)
    {
        isEmpty = false;
        characterName = name;
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

    //temporary before T4 template regeneration
    public RaceID RaceId => new RaceID("Human");
}
