using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitData {

	string Name { get; }
    int Level { get; }
    float Height { get; }
    /*float Size { get; set; }
    float Hp { get; set; }
    float Armor { get; set; }
    float Regen { get; set; }
    float Damage { get; set; }
    float ArmorIgnore { get; set; }
    float Reach { get; set; }
    float MoveSpeed { get; set; }
    float ViewRange { get; set; }
    float Height { get; set; }*/

    Attribute GetAttribute(Unit.AttributeType type);
}
