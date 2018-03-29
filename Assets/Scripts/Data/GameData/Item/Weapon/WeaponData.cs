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
    [SerializeField, InlineProperty, GUIColor(0.5f, 1, 0.5f)] protected Attribute dexReachBonus;
    [SerializeField, InlineProperty, GUIColor(0.5f, 0.5f, 1)] protected Attribute intRequired;
    [SerializeField, InlineProperty, GUIColor(0.5f, 0.5f, 1)] protected Attribute intDamageBonus;
    [SerializeField, InlineProperty, GUIColor(0.5f, 0.5f, 1)] protected Attribute intSpeedBonus;
    [SerializeField, InlineProperty, GUIColor(0.5f, 0.5f, 1)] protected Attribute intReachBonus;

    public int MinDamage
    {
        get { return (int)minDamage.Value; }
    }

    public int MaxDamage
    {
        get { return (int)maxDamage.Value; }
    }

    public float AverageDamage
    {
        get { return (float)(MinDamage + MaxDamage) / 2; }
    }

    public float Reach
    {
        get { return reach.Value; }
    }

    public float CastTime
    {
        get { return castTime.Value; }
    }

    public float AfterCastLockTime
    {
        get { return afterCastLockTime.Value; }
    }

    public float TotalAttackTime
    {
        get { return CastTime + AfterCastLockTime; }
    }

    public float AttacksPerSecond
    {
        get { return 1 / TotalAttackTime; }
    }

    public int StrRequired
    {
        get { return (int)strRequired.Value; }
    }

    public float StrDamageBonus
    {
        get { return strDamageBonus.Value; }
    }

    public float StrSpeedBonus
    {
        get { return strSpeedBonus.Value; }
    }

    public float StrReachBonus
    {
        get { return strReachBonus.Value; }
    }

    public int DexRequired
    {
        get { return (int)dexRequired.Value; }
    }

    public float DexDamageBonus
    {
        get { return dexDamageBonus.Value; }
    }

    public float DexSpeedBonus
    {
        get { return dexSpeedBonus.Value; }
    }

    public float DexReachBonus
    {
        get { return dexReachBonus.Value; }
    }

    public int IntRequired
    {
        get { return (int)intRequired.Value; }
    }

    public float IntDamageBonus
    {
        get { return intDamageBonus.Value; }
    }

    public float IntSpeedBonus
    {
        get { return intSpeedBonus.Value; }
    }

    public float IntReachBonus
    {
        get { return intReachBonus.Value; }
    }
}
