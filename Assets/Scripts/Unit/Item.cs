using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    private bool isEquipped;
    private Unit ownerUnit;
    private EquipmentSlotComponent slotComponent;

    [SerializeField] private string displayName;
    [SerializeField] private List<Skill> skills;
    [SerializeField] private List<Perk> perks;
    [SerializeField] private EquipmentSlot slot;
    [SerializeField] private ItemGraphics graphics;

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
            EquipmentSlotComponent[] slotComponents = unit.gameObject.GetComponentsInChildren<EquipmentSlotComponent>();
            foreach(EquipmentSlotComponent slotComp in slotComponents)
            {
                if(slotComp.SlotType == slot)
                {
                    slotComp.Equip(this);
                    slotComponent = slotComp;
                }
            }

            foreach(Perk perk in perks)
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
            foreach(Perk perk in perks)
            {
                ownerUnit.RemovePerk(perk);
            }
        }
    }

    public ItemGraphics Graphics
    {
        get { return graphics; }
    }
}
