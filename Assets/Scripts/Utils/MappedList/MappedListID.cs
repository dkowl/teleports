using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class MappedListID : IDeepCopyable {

    //TODO: make this a generic class for all IDs like ItemID, SkillID, PerkID, implement == operators
    [ValueDropdown("DropdownValues"), SerializeField] protected string name;

    public MappedListID()
    {
        name = string.Empty;
    }

    public MappedListID(string name)
    {
        this.name = name;
    }

    public object DeepCopy()
    {
        return new MappedListID(this);
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
        return name != null ? name.GetHashCode() : 0;
    }

    public static implicit operator string(MappedListID id)
    {
        return id.name;
    }

    public static implicit operator MappedListID (string str)
    {
        return new MappedListID(str);
    }

    public override string ToString()
    {
        return name;
    }

    protected virtual IList<string> DropdownValues()
    {
        return null;
    }

    public string Name => name;
}
