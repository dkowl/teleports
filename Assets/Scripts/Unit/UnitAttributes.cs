using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "unitAttributes", menuName = "Custom/UnitAttributes", order = 8)]
public class UnitAttributes : ScriptableObject {

    [System.Serializable]
    public class Element
    {
        public Element(Unit.AttributeType type)
        {
            attributeType = type;
        }
        [SerializeField]
        private Unit.AttributeType attributeType;

        public string name;
        public int rate;
    }

    [SerializeField]
    private Element[] elements = new Element[Enum.GetNames(typeof(Unit.AttributeType)).Length];

    public string GetNameOf(Unit.AttributeType type)
    {
        return "lol";
    }
	
    /*
    MEMORY DUMP: 
    Stays like it is, for stat bars: lvl1 avg value, lvl30 avg value, interpolate, use gaussian dist to calc slider value
    */
}
