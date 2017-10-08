using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotComponent : MonoBehaviour {

    [SerializeField]
    private EquipmentSlot slotType;

    public EquipmentSlot SlotType{
        get { return slotType; }
    }
}
