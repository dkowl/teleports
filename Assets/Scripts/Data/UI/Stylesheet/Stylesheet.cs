using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

public interface IStylesheetValue
{
    System.Type Type { get; }
}

public class StylesheetValue<T> : IStylesheetValue
{
    [SerializeField] private T value;

    public System.Type Type => typeof(T);

    public static explicit operator T(StylesheetValue<T> o)
    {
        return o.value;
    }
}

namespace StylesheetValues
{
    public class Float : StylesheetValue<float>{ }
    public class Color : StylesheetValue<UnityEngine.Color>{ }
}

public abstract class StylesheetKey
{
    [ValueDropdown("DropdownValues"), SerializeField]
    private string key;

    public abstract IList<string> DropdownValues();
}

namespace StylesheetKeys
{
    [System.Serializable]
    public class FontSize : StylesheetKey
    {
        public override IList<string> DropdownValues()
        {
            return Main.StaticData.UI.Stylesheet.GetKeys("Font Size");
        }
    }
}

[ShowOdinSerializedPropertiesInInspector]
public class StylesheetDictionary<T> where T : IStylesheetValue
{
    [OdinSerialize] private Dictionary<string, T> dictionary;

    public T GetValue(string key)
    {
        if (!dictionary.ContainsKey(key))
        {
            Debug.LogError($"No stylesheet data for key {key}");
            return default(T);
        }
        else
        {
            return dictionary[key];
        }
    }

    public IList<string> PresetNames()
    {
        var result = new List<string>();
        foreach (var key in dictionary.Keys)
        {
            result.Add(key);
        }
        return result;
    }
}

[System.Serializable]
[ShowOdinSerializedPropertiesInInspector]
public class Stylesheet : IStylesheet
{
    [OdinSerialize]
    private Dictionary<string, StylesheetDictionary<IStylesheetValue>> dictionaries;

    public T GetValue<T>(string typeKey, string key)
    {
        IStylesheetValue value = GetDictionary(typeKey)?.GetValue(key);
        Debug.Assert(value != null);

        if (typeof(T) != value.Type)
        {
            Debug.LogError($"Invalid stylesheet value type. Expected {typeof(T)}, found {value.Type}");
        }

        return (T)value;
    }

    public IList<string> GetKeys(string typeKey)
    {
        return GetDictionary(typeKey)?.PresetNames();
    }

    private StylesheetDictionary<IStylesheetValue> GetDictionary(string typeKey)
    {
        if (!dictionaries.ContainsKey(typeKey))
        {
            Debug.LogError($"Type {typeKey} not found in the stylesheet");
        }

        return dictionaries[typeKey];
    }
}


