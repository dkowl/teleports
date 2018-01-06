using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipMessage : IMessage {

    EventType eventType;
    ItemData item;
    EquipmentSlotType eqSlotType;

    public ItemEquipMessage(EventType eventType, ItemData item, EquipmentSlotType eqSlotType)
    {
        this.eventType = eventType;
        this.item = item;
        this.eqSlotType = eqSlotType;
    }

    public EventType Type
    {
        get { return eventType; }
    }

    public ItemData Item
    {
        get { return item; }
    }

    public EquipmentSlotType EqSlotType
    {
        get { return eqSlotType; }
    }

	public enum EventType
    {
        Equip,
        Unequip
    }
}
