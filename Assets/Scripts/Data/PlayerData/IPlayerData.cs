using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerData {

    string CharacterName { get; }
    string RaceName { get; }
    int Xp { get; set; }
    int Level { get; }
    int RankPoints { get; }
    TeleportData CurrentTeleportData { get; }

    void AddXp(int xpToAdd);
    SkillTreeSlot GetSkillTreeSlot(int id);
    int GetSkillTreeSlotLevel(int id);
    float GetStat(PlayerStats stat);
}
