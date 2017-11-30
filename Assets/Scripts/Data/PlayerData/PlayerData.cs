﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : IPlayerData
{
    private static readonly int SkillTreeSlotNo = 3;
    private static readonly int SkillSlotNo = 4;

    //main attributes
    [SerializeField] private string characterName;
    [SerializeField] private string raceName;
    [SerializeField] private int xp;
    [SerializeField] private int level = 1;
    [SerializeField] private int rankPoints;
    [SerializeField] private List<SkillID> skills;
    [SerializeField] private SkillTreeSlot[] skillTreeSlots;
    [SerializeField] private SkillID primarySkill;
    [SerializeField] private SkillID[] secondarySkills;
    [SerializeField] private UnitData liveUnitData;
    [SerializeField] private TeleportData teleportData;
    [SerializeField] private InventoryData inventoryData;

    public PlayerData(string name, string raceName)
    {
        characterName = name;
        this.raceName = raceName;
        xp = 0;
        level = 1;
        rankPoints = 0;
        skills = new List<SkillID>();
        skillTreeSlots = new SkillTreeSlot[SkillTreeSlotNo];
        for (int i = 0; i < skillTreeSlots.Length; i++)
        {
            skillTreeSlots[i] = new SkillTreeSlot();
        }
        primarySkill = MainData.CurrentGameData.GetRace(raceName).BaseStats.MainAttack;
        secondarySkills = new SkillID[SkillSlotNo];
        for (int i = 0; i < secondarySkills.Length; i++)
        {
            secondarySkills[i] = new SkillID();
        }
        liveUnitData = MainData.CurrentGameData.GetRace(raceName).BaseStats;
        teleportData = new TeleportData();
        inventoryData = new InventoryData();
        CorrectInvalidData();
    }

    public void CorrectInvalidData()
    {
        if (PlayerDataValidator.ValidateName(characterName) != PlayerDataValidator.NameValidationResult.OK)
        {
            characterName = "";
        }
        if(level < 1)
        {
            level = 1;
        }
        if(skillTreeSlots.Length < SkillTreeSlotNo)
        {
            skillTreeSlots = new SkillTreeSlot[SkillTreeSlotNo];
            for(int i = 0; i<skillTreeSlots.Length; i++)
            {
                skillTreeSlots[i] = new SkillTreeSlot();
            }
        }
        if(secondarySkills.Length < SkillSlotNo)
        {
            secondarySkills = new SkillID[SkillSlotNo];
            for(int i = 0; i<secondarySkills.Length; i++)
            {
                secondarySkills[i] = new SkillID();
            }
        }
        teleportData.CorrectInvalidData();
        inventoryData.CorrectInvalidData();
    }

    #region interface implementation
    #region properties
    public string CharacterName
    {
        get
        {
            return characterName;  
        }
    }

    public string RaceName
    {
        get { return raceName; }
    }

    public int Xp
    {
        get
        {
            return xp;
        }
        set
        {
            if(value >= xp)
            {
                AddXp(value - xp);
            }
        }
    }

    public int Level
    {
        get
        {
            level = Levels.xp.Level(xp);
            return level;
        }
    }

    public int RankPoints
    {
        get
        {
            return rankPoints;
        }
    }

    public UnitData BaseUnitData
    {
        get
        {
            return MainData.CurrentGameData.GetRace(RaceName).BaseStats;
        }
    }

    public UnitData LiveUnitData
    {
        get
        {
            if(!liveUnitData.IsInitialized)
            {
                liveUnitData = MainData.CurrentGameData.GetRace(raceName).BaseStats;
            }

            return liveUnitData;
        }
    }

    public TeleportData CurrentTeleportData
    {
        get { return teleportData; }
    }

    public InventoryData InventoryData
    {
        get { return inventoryData; }
    }

    public SkillID PrimarySkillId
    {
        get { return primarySkill; }
    }
    #endregion

    #region methods
    /// <summary>
    /// While adding XP this method handles leveling up and updates rank points.
    /// </summary>
    /// <param name="xpToAdd"></param>
    public void AddXp(int xpToAdd)
    {
        UpdateRankPoints(xpToAdd);
        /*while(xpToAdd >= RequiredXp)
        {
            xp += RequiredXp;
            xpToAdd -= RequiredXp;
            LevelUp();
        }*/
        xp += xpToAdd;                
    }

    public SkillTreeSlot GetSkillTreeSlot(int id)
    {
        if(id < SkillTreeSlotNo)
        {
            return skillTreeSlots[id];
        }
        else
        {
            return null;
        }
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
                return LiveUnitData.GetAttribute(UnitAttributes.Type.Hp).Value;
            case PlayerStats.Armor:
                return LiveUnitData.GetAttribute(UnitAttributes.Type.Armor).Value;
            case PlayerStats.ArmorIgnore:
                return 0; //TODO
            case PlayerStats.Damage:
                return 0; //TODO
            case PlayerStats.MoveSpeed:
                return LiveUnitData.GetAttribute(UnitAttributes.Type.MoveSpeed).Value;
            case PlayerStats.Reach:
                return LiveUnitData.GetAttribute(UnitAttributes.Type.Reach).Value;
            case PlayerStats.Regen:
                return LiveUnitData.GetAttribute(UnitAttributes.Type.Regen).Value;
            case PlayerStats.ViewRange:
                return LiveUnitData.GetAttribute(UnitAttributes.Type.ViewRange).Value;
            case PlayerStats.DamagePerSecond:
                return 0; //TODO: Divide by attack speed
            case PlayerStats.TeleportPower:
                return CurrentTeleportData.Power;
            case PlayerStats.TeleportTime:
                return CurrentTeleportData.Time;
            default:
                return 0.0f;
        }
    }
    #endregion
    #endregion

    private int CurrentXp
    {
        get
        {
            return Levels.xp.Current(xp);
        }
    }

    private int RequiredXp
    {
        get
        {
            return Levels.xp.Required(xp);
        }
    }

    //called by AddXp
    private void LevelUp()
    {
        level++;
    }

    //called by AddXp
    private void UpdateRankPoints(int score)
    {
        rankPoints = RankPointUpdater.UpdateRankPoints(rankPoints, score);
    }
}
