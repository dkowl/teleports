using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : UniqueScriptableObject {

    [SerializeField] private List<Perk> perks;
    [SerializeField] private List<Skill> skills;
    [SerializeField] private EquipmentSlot slot;

}
