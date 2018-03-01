using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using Teleports.Utils;

public partial class PlayerData
{
    private static readonly int SkillTreeSlotNo = 3;
    private static readonly int SkillSlotNo = 4;
    
    public PlayerData(string name, string raceName)
    {
        Init(name, raceName);
    }

    private void Init(string name, string raceName)
    {
        characterName = name;
        xp = 0;
        level = 1;
        rankPoints = 0;
        skills = new List<SkillID>();
        Utils.InitWithNew(ref skillTreeSlots, SkillTreeSlotNo);
        primarySkill = MainData.Game.GetRace(raceName).BaseStats.MainAttack;
        Utils.InitWithNew(ref secondarySkills, SkillSlotNo);
        unitData = MainData.Game.GetRace(raceName).BaseStats;
        teleportData = new TeleportData();
    }

    public void SetXp(int value)
    {
        if (value >= xp)
        {
            AddXp(value - xp);
        }
    }

    public void CorrectInvalidData()
    {
        if (PlayerDataValidator.ValidateName(characterName) != PlayerDataValidator.NameValidationResult.OK)
        {
            Debug.LogWarning("Invalid player name: " + characterName + ". Changing to 'New Player'");
            characterName = "New Player";
        }
        if(level < 1)
        {
            Debug.LogWarning("Invalid player level: " + level.ToString() + ". Changing to '1'");
            level = 1;
        }
        if(skillTreeSlots == null || skillTreeSlots.Count < SkillTreeSlotNo)
        {
            Debug.LogWarning("Skill tree slots not found, initializing...");
            Utils.InitWithNew(ref skillTreeSlots, SkillTreeSlotNo);
        }
        if(secondarySkills == null || secondarySkills.Count < SkillSlotNo)
        {
            Debug.LogWarning("Secondary skill not found, initializing...");
            Utils.InitWithNew(ref secondarySkills, SkillSlotNo);
        }
        if(unitData == null)
        {
            Debug.LogWarning("Unit data not found, substituting with Human base data");
            unitData = MainData.Game.GetRace("Human").BaseStats;
        }
        unitData.CorrectInvalidData();
        if (teleportData == null)
        {
            Debug.LogWarning("Teleport data not found");
            teleportData = new TeleportData();
        }
        teleportData.CorrectInvalidData();
    }

    public string RaceName => unitData.RaceName;
    public UnitData BaseUnitData => MainData.Game.GetRace(RaceName).BaseStats;

    public void AddXp(int xpToAdd)
    {
        xp += xpToAdd;
        UpdateLevel();
        UpdateRankPoints(xpToAdd);
    }

    public SkillTreeSlot GetSkillTreeSlot(int id)
    {
        return id < SkillTreeSlotNo ? skillTreeSlots[id] : null;
    }

    public int GetSkillTreeSlotLevel(int id)
    {
        int result = 0;

        foreach(SkillID skillID in skills)
        {
            if (skillID.treeID == id) result++;
        }

        return result;
    }

    public float GetStat(PlayerStats type)
    {
        switch (type)
        {
            case PlayerStats.Hp:
                return UnitData.GetAttribute(UnitAttributes.Type.Hp).Value;
            case PlayerStats.Armor:
                return UnitData.GetAttribute(UnitAttributes.Type.Armor).Value;
            case PlayerStats.ArmorIgnore:
                return 0; //TODO
            case PlayerStats.Damage:
                return 0; //TODO
            case PlayerStats.MoveSpeed:
                return UnitData.GetAttribute(UnitAttributes.Type.MoveSpeed).Value;
            case PlayerStats.Reach:
                return UnitData.GetAttribute(UnitAttributes.Type.Reach).Value;
            case PlayerStats.Regen:
                return UnitData.GetAttribute(UnitAttributes.Type.Regen).Value;
            case PlayerStats.ViewRange:
                return UnitData.GetAttribute(UnitAttributes.Type.ViewRange).Value;
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

    //called by AddXp
    private void UpdateLevel()
    {
        level = Levels.xp.Level(xp);
    }

    //called by AddXp
    private void UpdateRankPoints(int score)
    {
        rankPoints = RankPointUpdater.UpdateRankPoints(rankPoints, score);
    }
}
