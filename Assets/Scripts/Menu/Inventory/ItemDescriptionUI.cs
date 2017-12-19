using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

[ExecuteInEditMode]
public class ItemDescriptionUI : MonoBehaviour {

    [SerializeField] private Text text;
    private InventoryMenu parentMenu;
    private ItemData itemData;
    [SerializeField] private UnitWeaponCombiner combiner;

    void Awake()
    {
        parentMenu = GetComponentInParent<InventoryMenu>();
        Debug.Assert(parentMenu != null);
    }

    private Color StatValueTextColor(float baseValue, float strBonus, float dexBonus, float intBonus)
    {
        float total = baseValue + strBonus + dexBonus + intBonus;
        float baseComponent = baseValue / total;
        float colorComponent = 1 - baseComponent;
        float maxBonus = Mathf.Max(strBonus, dexBonus, intBonus);
        return new Color(
            baseComponent + colorComponent * (strBonus / maxBonus),
            baseComponent + colorComponent * (dexBonus / maxBonus),
            baseComponent + colorComponent * (intBonus / maxBonus)
            );
    }

    private Color StatValueTextColor(WeaponData weaponData, UnitWeaponCombiner.AbilityStatBonus bonus)
    {
        float baseValue = 0;
        if(bonus is UnitWeaponCombiner.DamageBonus)
        {
            baseValue = weaponData.AverageDamage;
        }
        else if(bonus is UnitWeaponCombiner.SpeedBonus)
        {
            baseValue = weaponData.TotalAttackTime;
        }
        else if(bonus is UnitWeaponCombiner.ReachBonus)
        {
            baseValue = weaponData.Reach;
        }
        return StatValueTextColor(baseValue, bonus.StrComponent, bonus.DexComponent, bonus.IntComponent);
    }

    private string DamageBonusInfoString(WeaponData weaponData, UnitWeaponCombiner.DamageBonus bonus)
    {
        string result = "";
        if(bonus.Value > 0)
        {
            result += string.Format("({0} - {1} ", weaponData.MinDamage, weaponData.MaxDamage);
            if(bonus.StrComponent > 0)
            {
                result += string.Format("<color=red>+ {0:F0}</color>", bonus.StrComponent);
            }
            result += ")";
        }
        return result;
        
    }

    public ItemData ItemData
    {
        set
        {
            if (value != itemData)
            {
                itemData = value;
            }
            else
            {
                return;
            }

            if(itemData == null)
            {
                text.text = "";
                return;
            }

            if (itemData.IsType(ItemType.Weapon))
            {
                WeaponData weaponData = itemData.WeaponData;
                UnitData unitData = parentMenu.UnitData;
                combiner = new UnitWeaponCombiner(unitData, weaponData);
                if (combiner.CanUse)
                {
                    text.text = string.Format(
                        "<size=+24>{0:F1}</size> damage / second\n" +
                        "<size=+4><#{1}>{2} - {3}</color></size> damage {4}",
                        combiner.DamagePerSecond,
                        ColorUtility.ToHtmlStringRGB(StatValueTextColor(weaponData, combiner.DamageBonusData)),
                        combiner.MinDamage,
                        combiner.MaxDamage,
                        DamageBonusInfoString(weaponData, combiner.DamageBonusData)
                        );
                }
                else
                {
                    text.text = "Locked";
                }
                    
            }

        }
    }
}
