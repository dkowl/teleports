using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "playerData", menuName = "Custom/PlayerData", order = 6)]
public class PlayerData : ScriptableObject, IPlayerData
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
    [SerializeField] private SkillTreeSlot[] skillTreeSlots = new SkillTreeSlot[SkillTreeSlotNo];
    [SerializeField] private SkillID primarySkill;
    [SerializeField] private SkillID[] secondarySkills = new SkillID[SkillSlotNo];
    [SerializeField] private UnitData liveUnitData = null;

    #region interface implementation
    #region properties
    public string CharacterName
    {
        get
        {
            return characterName;  
        }
    }

    public int Xp
    {
        get
        {
            return xp;
        }
        set
        {
            if(value > xp)
            {
                AddXp(value - xp);
            }
        }
    }

    public int Level
    {
        get
        {
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
    #endregion

    #region methods
    /// <summary>
    /// While adding XP this method handles leveling up and updates rank points.
    /// </summary>
    /// <param name="xpToAdd"></param>
    public void AddXp(int xpToAdd)
    {
        UpdateRankPoints(xpToAdd);
        while(xpToAdd >= RequiredXp)
        {
            xp += RequiredXp;
            xpToAdd -= RequiredXp;
            LevelUp();
        }
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
                return LiveUnitData.GetAttribute(Unit.AttributeType.Hp).Value;
            case PlayerStats.Armor:
                return LiveUnitData.GetAttribute(Unit.AttributeType.Armor).Value;
            case PlayerStats.ArmorIgnore:
                return LiveUnitData.GetAttribute(Unit.AttributeType.ArmorIgnore).Value;
            case PlayerStats.Damage:
                return LiveUnitData.GetAttribute(Unit.AttributeType.Damage).Value;
            case PlayerStats.MoveSpeed:
                return LiveUnitData.GetAttribute(Unit.AttributeType.MoveSpeed).Value;
            case PlayerStats.Reach:
                return LiveUnitData.GetAttribute(Unit.AttributeType.Reach).Value;
            case PlayerStats.Regen:
                return LiveUnitData.GetAttribute(Unit.AttributeType.Regen).Value;
            case PlayerStats.ViewRange:
                return LiveUnitData.GetAttribute(Unit.AttributeType.ViewRange).Value;
            case PlayerStats.DamagePerSecond:
                return LiveUnitData.GetAttribute(Unit.AttributeType.Damage).Value; //TODO: Divide by attack speed
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
            return XpLevels.CurrentXp(xp);
        }
    }

    private int RequiredXp
    {
        get
        {
            return XpLevels.RequiredXp(xp);
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
