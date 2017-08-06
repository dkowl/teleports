using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerData {

    string CharacterName { get; }
    int Xp { get; set; }
    int Level { get; }
    int RankPoints { get; }

    void AddXp(int xpToAdd);
    SkillTreeSlot GetSkillTreeSlot(int id);
    int GetSkillTreeSlotLevel(int id);
}
