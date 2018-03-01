using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UnitAttributesData {
    public UnitAttributesData()
    {
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
}
