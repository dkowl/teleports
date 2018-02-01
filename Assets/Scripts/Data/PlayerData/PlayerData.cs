using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : IPlayerData
{
    private static readonly int SkillTreeSlotNo = 3;
    private static readonly int SkillSlotNo = 4;

    //main attributes
    [SerializeField] private string characterName;
    [SerializeField] private int xp;
    [SerializeField] private int level = 1;
    [SerializeField] private int rankPoints;
    [SerializeField] private List<SkillID> skills;
    [SerializeField] private SkillTreeSlot[] skillTreeSlots;
    [SerializeField] private SkillID primarySkill;
    [SerializeField] private SkillID[] secondarySkills;
    [SerializeField] private UnitData unitData;
    [SerializeField] private TeleportData teleportData;

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
        unitData = MainData.CurrentGameData.GetRace(raceName).BaseStats;
        teleportData = new TeleportData();
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
        if(skillTreeSlots.Length < SkillTreeSlotNo)
        {
            Debug.LogWarning("Not enough skill tree slots");
            skillTreeSlots = new SkillTreeSlot[SkillTreeSlotNo];
            for(int i = 0; i<skillTreeSlots.Length; i++)
            {
                skillTreeSlots[i] = new SkillTreeSlot();
            }
        }
        if(secondarySkills == null)
        {
            Debug.LogWarning("Secondary skill not found, initializing...");
            secondarySkills = new SkillID[SkillSlotNo];
        }
        if(secondarySkills.Length < SkillSlotNo)
        {
            Debug.LogWarning("Not enough secondary skill slots");
            secondarySkills = new SkillID[SkillSlotNo];
            for(int i = 0; i<secondarySkills.Length; i++)
            {
                secondarySkills[i] = new SkillID();
            }
        }
        if(unitData == null)
        {
            Debug.LogWarning("Unit data not found, substituting with Human base data");
            unitData = MainData.CurrentGameData.GetRace("Human").BaseStats;
        }
        if (teleportData == null)
        {
            Debug.LogWarning("Teleport data not found");
            teleportData = new TeleportData();
        }
        teleportData.CorrectInvalidData();
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
        get { return unitData.RaceName; }
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

    public UnitData UnitData
    {
        get
        {
            if(!unitData.IsInitialized)
            {
                unitData = MainData.CurrentGameData.GetRace(RaceName).BaseStats;
            }

            return unitData;
        }
    }

    public TeleportData CurrentTeleportData
    {
        get { return teleportData; }
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
