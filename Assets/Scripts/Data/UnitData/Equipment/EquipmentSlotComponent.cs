using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotComponent : MonoBehaviour {

    [SerializeField]
    private EquipmentSlotType slotType;

    private GameObject itemObject;
    private bool isEmpty = true;

    public void Equip(ItemData itemData)
    {
        Unequip();

        itemObject = Instantiate(Main.StaticData.Graphics.ItemGraphics.GetValue(itemData.GraphicsId).Prefab, transform);
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

    public EquipmentSlotType SlotType => slotType;
    public bool IsEmpty => isEmpty;
}
