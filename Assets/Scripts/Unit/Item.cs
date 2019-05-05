using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    private bool isEquipped;
    private Unit ownerUnit;
    private EquipmentSlotComponent slotComponent;
    private EquipmentSlotType primarySlot;

    [SerializeField]
    private ItemData data;

    public void Start()
    {
        Unit unit = GetComponentInParent<Unit>();
        Equip(unit);
    }

    public void OnDisable()
    {
        Unequip();
    }

    public void Equip(Unit unit)
    {
        if (isEquipped) return;

        slotComponent = ItemSpawner.Spawn(unit.gameObject, data, primarySlot);

        foreach(PerkID perkId in data.Perks)
        {
            unit.AddPerk(Main.StaticData.Game.Perks.GetValue(perkId));
        }

        isEquipped = true;
        ownerUnit = unit;
    }

    public void Unequip()
    {
        if (!isEquipped || ownerUnit == null) return;

        slotComponent?.Unequip();
        foreach(PerkID perkId in data.Perks)
        {
            ownerUnit.RemovePerk(Main.StaticData.Game.Perks.GetValue(perkId));
        }
    }

    public ItemData Data
    {
        get { return data; }
        set { data = value; }
    }

    public EquipmentSlotType PrimarySlot
    {
        set { primarySlot = value; }
    }
}
