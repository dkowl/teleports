using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "race", menuName = "Custom/Race", order = 4)]
public class Race : ScriptableObject {

    public string name_;
    public UnitData baseStats_;
}
