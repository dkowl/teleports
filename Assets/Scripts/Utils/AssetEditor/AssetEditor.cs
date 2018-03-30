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
    private void AddItemGraphicsToGraphicsData()
    {
        var graphicsDataAssetInfo = GetAllAssetInfosOfType<GraphicsData>()[0];
        var itemGraphics = GetAllAssetInfosOfType<ItemGraphics>();

        GraphicsData graphicsData = graphicsDataAssetInfo.AssetObject;

        /*print(graphicsDataAssetInfo.Path);
        var graphicsDataPt = new PropertyTree<MappedList<ItemGraphics>>(new MappedList<ItemGraphics>[]{ graphicsDataAssetInfo.AssetObject.ItemGraphics});
        print(graphicsDataPt.RootPropertyCount);
        foreach(var target in graphicsDataPt.Targets) print(target);
        for (int i = 0; i < graphicsDataPt.RootPropertyCount; i++)
        {
            print(graphicsDataPt.GetRootProperty(i));
        }
        foreach (var property in graphicsDataPt.EnumerateTree(true))
        {
            print(property.Name);
        }
        var sp = graphicsDataPt.GetPropertyAtPath("list");
        print(sp == null);*/
        /*print(sp.isArray);
        sp.arraySize = itemGraphics.Count;
        for (int i = 0; i < itemGraphics.Count; i++)
        {
            sp.InsertArrayElementAtIndex(i);
            var element = sp.GetArrayElementAtIndex(i);
            element.objectReferenceValue = graphicsDataAssetInfo.AssetObject;
        }*/
        //graphicsDataSo.ApplyModifiedProperties();
        //AssetDatabase.SaveAssets();
    }

    [Button]
    private void Test()
    {
        print(typeof(ItemGraphics).ToString());
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
