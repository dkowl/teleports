using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class WeaponData {

	[SerializeField, InlineProperty, GUIColor(1, 0.5f, 0.5f)] protected Attribute minDamage;
    [SerializeField, InlineProperty, GUIColor(1, 0.5f, 0.5f)] protected Attribute maxDamage;
    [SerializeField, InlineProperty, GUIColor(0.5f, 0.5f, 1)] protected Attribute reach;
    [SerializeField, InlineProperty, GUIColor(0.5f, 1, 0.5f)] protected Attribute castTime;
    [SerializeField, InlineProperty, GUIColor(0.5f, 1, 0.5f)] protected Attribute afterCastLockTime;
    [SerializeField, InlineProperty, GUIColor(1, 0.5f, 0.5f)] protected Attribute strRequired;
    [SerializeField, InlineProperty, GUIColor(1, 0.5f, 0.5f)] protected Attribute strDamageBonus;
    [SerializeField, InlineProperty, GUIColor(1, 0.5f, 0.5f)] protected Attribute strSpeedBonus;
    [SerializeField, InlineProperty, GUIColor(1, 0.5f, 0.5f)] protected Attribute strReachBonus;
    [SerializeField, InlineProperty, GUIColor(0.5f, 1, 0.5f)] protected Attribute dexRequired;
    [SerializeField, InlineProperty, GUIColor(0.5f, 1, 0.5f)] protected Attribute dexDamageBonus;
    [SerializeField, InlineProperty, GUIColor(0.5f, 1, 0.5f)] protected Attribute dexSpeedBonus;
    [SerializeField, InlineProperty, GUIColor(0.5f, 0.5f, 1)] protected Attribute intRequired;
    [SerializeField, InlineProperty, GUIColor(0.5f, 0.5f, 1)] protected Attribute intDamageBonus;
    [SerializeField, InlineProperty, GUIColor(0.5f, 0.5f, 1)] protected Attribute intSpeedBonus;
    [SerializeField, InlineProperty, GUIColor(0.5f, 0.5f, 1)] protected Attribute intReachBonus;

}
