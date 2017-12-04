using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotComponent : MonoBehaviour {

    [SerializeField]
    private EquipmentSlotType slotType;

    private GameObject itemObject = null;
    private bool isEmpty = true;

    public EquipmentSlotType SlotType{
        get { return slotType; }
    }

    public void Equip(ItemData itemData)
    {
        Unequip();

        itemObject = Instantiate(itemData.Graphics.Prefab, transform);
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

    public bool IsEmpty
    {
        get { return isEmpty; }
    }
}
