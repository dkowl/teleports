using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotComponent : MonoBehaviour {

    [SerializeField]
    private EquipmentSlot slotType;

    private GameObject itemObject = null;
    private bool isEmpty = true;

    public EquipmentSlot SlotType{
        get { return slotType; }
    }

    public void Equip(Item item)
    {
        Unequip();

        itemObject = Instantiate(item.Graphics.Prefab, transform);
        isEmpty = false;
    }

    public void Unequip()
    {
        if (itemObject != null)
        {
            Destroy(itemObject);
        }
        isEmpty = true;
    }
}
