using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

public class AssetEditor : MonoBehaviour
{

    private static AssetEditor instance;

    public void Awake()
    {
        if(instance != null && instance != this) Destroy(instance);
        instance = this;
    }

    [Button]
    private void AddItemGraphicsNames()
    {
        var itemGraphics = GetAllAssetInfosOfType<ItemGraphics>();
        print(itemGraphics.Count);
        foreach (var asset in itemGraphics)
        {
            print(asset.Name);
            SerializedObject so = new SerializedObject(asset.AssetObject);
            SerializedProperty sp = so.FindProperty("uniqueName");
            sp.stringValue = asset.Name;
            so.ApplyModifiedProperties();
        }
        AssetDatabase.SaveAssets();
    }

    [Button]
    private void NameRaceGraphics()
    {
        var raceGraphicsInfos = GetAllAssetInfosOfType<RaceGraphics>();
        foreach (var assetInfo in raceGraphicsInfos)
        {
            assetInfo.SetUniqueNameToFilename();
        }
    }

    public List<AssetInfo<T>> GetAllAssetInfosOfType<T>() where T : Object
    {
        string type = typeof(T).ToString();
        var guids = AssetDatabase.FindAssets("t:" + type);
        var result = new List<AssetInfo<T>>();
        foreach (var guid in guids)
        {
            result.Add(new AssetInfo<T>(guid));
        }
        return result;
    }

    public List<T> GetAllAssetsOfType<T>() where T : Object
    {
        string type = typeof(T).ToString();
        var guids = AssetDatabase.FindAssets("t:" + type);
        var result = new List<T>();
        foreach (var guid in guids)
        {
            result.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));
        }
        return result;
    }

    public static AssetEditor Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("AssetEditor");
                instance = go.AddComponent<AssetEditor>();
            }
            return instance;
        }
    }
	
}
