using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemSpawner{

	public static EquipmentSlotComponent Spawn(GameObject parentObject, ItemData itemData)
    {
        EquipmentSlotComponent[] slotComponents = parentObject.GetComponentsInChildren<EquipmentSlotComponent>();
        foreach (EquipmentSlotComponent slotComp in slotComponents)
        {
            if (slotComp.SlotType == itemData.Slot)
            {
                slotComp.Equip(itemData);
                return slotComp;
            }
        }

        return null;
    }


}
