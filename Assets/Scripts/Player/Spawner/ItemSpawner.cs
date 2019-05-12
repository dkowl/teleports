using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemSpawner{

	public static EquipmentSlotComponent Spawn(GameObject parentObject, ItemData itemData, EquipmentSlotType slotType)
    {
        EquipmentSlotComponent[] slotComponents = parentObject.GetComponentsInChildren<EquipmentSlotComponent>();
        foreach (EquipmentSlotComponent slotComp in slotComponents)
        {
            if (slotComp.SlotType == slotType)
            {
                slotComp.Equip(itemData);
                return slotComp;
            }
        }
        Debug.LogWarning("Cannot find the right EquipmentSlotComponent");
        return null;
    }


}
