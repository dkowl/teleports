using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitData {

	string Name { get; }
    int Level { get; }
    float Size { get; }
    float Hp { get; }
    float Armor { get; }
    float Regen { get; }
    float Damage { get; }
    float ArmorIgnore { get; }
    float Reach { get; }
    float MoveSpeed { get; }
    float ViewRange { get; }
    float Height { get; }
}
