using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EquipmentSlotData", menuName = "Data/Equipment/SlotData")]
public class EquipmentSlotData : ScriptableObject {

    [SerializeField]
    private EquipmentSlot slotType;

    [SerializeField]
    private string boneName;

    public EquipmentSlot SlotType
    {
        get { return slotType; }
    }
}
