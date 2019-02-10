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
    public class Sprite : StylesheetValue<UnityEngine.Sprite> { }
}

public abstract class StylesheetKey
{
    [ValueDropdown("DropdownValues"), SerializeField]
    private string key;

    public abstract string PresetType();
    public IList<string> DropdownValues()
    {
        return Main.StaticData.UI.Stylesheet.GetKeys(PresetType());
    }

    public string String
    {
        get { return key; }
        set { key = value; }
    }
}

namespace StylesheetKeys
{
    [System.Serializable]
    public class FontSize : StylesheetKey
    {
        public override string PresetType()
        {
            return "Font Size";
        }
    }

    [System.Serializable]
    public class TextColor : StylesheetKey
    {
        public override string PresetType()
        {
            return "Text Color";
        }
    }

    [System.Serializable]
    public class Color : StylesheetKey
    {
        public override string PresetType()
        {
            return "Color";
        }
    }

    public class Sprite : StylesheetKey
    {
        public override string PresetType()
        {
            return "Sprite";
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

    public T GetValue<T>(StylesheetKey key)
    {
        return GetValue<T>(key.PresetType(), key.String);
    }

    private T GetValue<T>(string typeKey, string key)
    {
        var value = GetDictionary(typeKey)?.GetValue(key) as StylesheetValue<T>;
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


