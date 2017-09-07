using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "saveData", menuName = "Custom/SaveData", order = 6)]
public class PlayerData : ScriptableObject, IPlayerData
{
    private static readonly int SkillTreeSlotNo = 3;

    //main attributes
    [SerializeField] private string characterName;
    [SerializeField] private int xp;
    [SerializeField] private int level = 1;
    [SerializeField] private int rankPoints;
    [SerializeField] private List<SkillID> skills;
    [SerializeField] private SkillTreeSlot[] skillTreeSlots = new SkillTreeSlot[SkillTreeSlotNo];

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
        return 1.0f;
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
