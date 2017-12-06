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
        if (!isEquipped)
        {
            slotComponent = ItemSpawner.Spawn(unit.gameObject, data, primarySlot);

            foreach(Perk perk in data.Perks)
            {
                unit.AddPerk(perk);
            }

            isEquipped = true;
            ownerUnit = unit;
        }
    }

    public void Unequip()
    {
        if (isEquipped && ownerUnit != null)
        {
            slotComponent.Unequip();
            foreach(Perk perk in data.Perks)
            {
                ownerUnit.RemovePerk(perk);
            }
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
