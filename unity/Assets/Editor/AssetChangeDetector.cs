using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetChangeDetector : UnityEditor.AssetModificationProcessor {

    static string[] OnWillSaveAssets(string[] paths)
    {
        Debug.Log("OnWillSaveAssets");
        foreach (string path in paths)
        {
            IChangeDetectableAsset asset = AssetDatabase.LoadAssetAtPath<Object>(path) as IChangeDetectableAsset;
            if (asset != null)
            {
                asset.OnWillSaveAssets();
            }            
        }
        return paths;
    }
}
