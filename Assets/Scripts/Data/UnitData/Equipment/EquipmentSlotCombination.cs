using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentSlotCombination {

	[SerializeField] private EquipmentSlotType primarySlot;
    [SerializeField] private EquipmentSlotType[] slotsTaken;

    public IList<EquipmentSlotType> SlotsTaken
    {
        get { return System.Array.AsReadOnly(slotsTaken); }
    }

    public EquipmentSlotType PrimarySlot
    {
        get { return primarySlot; }
    }
}
