using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UnitAttributesData {
    public UnitAttributesData()
    {
        strength = new Attribute(10);
        dexterity = new Attribute(10);
        intelligence = new Attribute(10);
        size = new Attribute(0.5f);
        height = new Attribute(1.8f);
        healthPoints = new Attribute(100);
        armor = new Attribute(0);
        regeneration = new Attribute(0);
        movementSpeed = new Attribute(10);
        rotationSpeed = new Attribute(2);
        reach = new Attribute(0);
        viewRange = new Attribute(30);
    }

    public void CorrectInvalidData()
    {
        if(strength == null) strength = new Attribute(10);
        if (dexterity == null) dexterity = new Attribute(10);
        if (intelligence == null) intelligence = new Attribute(10);
    }
}
