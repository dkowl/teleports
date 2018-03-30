using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetInfo <T> where T : Object
{

    private T assetObject;
    private string guid;
    private string path;
    private string name;

    public AssetInfo(string guid)
    {
        this.guid = guid;
        path = AssetDatabase.GUIDToAssetPath(guid);
        assetObject = AssetDatabase.LoadAssetAtPath<T>(path);
        name = System.IO.Path.GetFileNameWithoutExtension(path);
    }

    public T AssetObject => assetObject;
    public string Path => path;
    public string Name => name;
}
