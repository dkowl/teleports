using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
#endif

public class AssetEditor : Singleton<AssetEditor>
{
#if UNITY_EDITOR
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
        NameUniqueAssets<RaceGraphics>();
    }

    [Button]
    private void NameAnimationClipDatas()
    {
        NameUniqueAssets<AnimationClipData>();
    }

    [Button]
    private void NameSkillGraphics()
    {
        NameUniqueAssets<SkillGraphics>();
    }

    public static void AddAssetsOfType<T>(Object o, MappedList<T> graphicsList) where T : Object, IUniqueName
    {
        graphicsList.ClearList();
        graphicsList.AddItems(Instance.GetAllAssetsOfType<T>());
        graphicsList.MakeDict();
        EditorUtility.SetDirty(o);
        AssetDatabase.SaveAssets();
    }

    public static void AddScriptableObjectWrappedDataOfType<T, TWrapper>(Object o, MappedList<T> dataList)
        where T : IUniqueName
        where TWrapper : ScriptableObjectDataWrapper<T>
    {
        dataList.ClearList();
        var dataWrappers = Instance.GetAllAssetsOfType<TWrapper>();
        var data = new List<T>();
        foreach (var dataWrapper in dataWrappers)
        {
            data.Add(dataWrapper.Data);
        }
        dataList.AddItems(data);
        dataList.MakeDict();
        EditorUtility.SetDirty(o);
        AssetDatabase.SaveAssets();
    }

    private void NameUniqueAssets<T>() where T : Object, IUniqueName
    {
        var assetInfos = GetAllAssetInfosOfType<T>();
        foreach (var assetInfo in assetInfos)
        {
            assetInfo.SetUniqueNameToFilename(false, false);
        }
        AssetDatabase.SaveAssets();
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
#endif
}
