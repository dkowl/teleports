using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipMessage : IMessage {

    EventType eventType;
    ItemData item;

    public ItemEquipMessage(EventType eventType, ItemData item)
    {
        this.eventType = eventType;
        this.item = item;
    }

    public EventType Type
    {
        get { return eventType; }
    }

    public ItemData Item
    {
        get { return item; }
    }

	public enum EventType
    {
        Equip,
        Unequip
    }
}
