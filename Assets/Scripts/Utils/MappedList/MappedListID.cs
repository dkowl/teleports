using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public abstract class MappedListID {

    //TODO: make this a generic class for all IDs like ItemID, SkillID, PerkID, implement == operators
    [ValueDropdown("DropdownValues"), SerializeField] private string name;

    public MappedListID()
    {
        name = string.Empty;
    }

    public MappedListID(string name)
    {
        this.name = name;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var id = (MappedListID)obj;
        return name == id.name;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }

    public static implicit operator string(MappedListID id)
    {
        return id.name;
    }

    protected abstract IList<string> DropdownValues();

    public string Name
    {
        get { return name; }
    }
}
