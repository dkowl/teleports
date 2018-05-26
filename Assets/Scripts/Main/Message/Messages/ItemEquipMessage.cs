using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipMessage : IMessage {

    private EventType eventType;
    private ItemData item;
    private EquipmentSlotType eqSlotType;

    public ItemEquipMessage(EventType eventType, ItemData item, EquipmentSlotType eqSlotType)
    {
        this.eventType = eventType;
        this.item = item;
        this.eqSlotType = eqSlotType;
    }

    public EventType Type => eventType;
    public ItemData Item => item;
    public EquipmentSlotType EqSlotType => eqSlotType;

    public enum EventType
    {
        Equip,
        Unequip
    }
}
