using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

[ExecuteInEditMode]
public class ItemDescriptionUI : MonoBehaviour {

    [SerializeField] private Text text;

    private ItemData itemData;


    public ItemData ItemData
    {
        set
        {
            if (value != itemData)
            {
                itemData = value;
                Debug.Log("CO1");
            }
            else
            {
                Debug.Log("CO2");
                return;
            }

            if(itemData == null)
            {
                Debug.Log("CO3");
                text.text = "";
                return;
            }

            if (itemData.IsType(ItemType.Weapon))
            {
                Debug.Log("CO");
                WeaponData weaponData = itemData.WeaponData;
                text.text =
                    "Damage: " + weaponData.MinDamage.ToString() + " - " + weaponData.MaxDamage.ToString();
            }

        }
    }
}
