using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemBrowserWindow : EditorWindow {

    

    [MenuItem("Teleports/ItemBrowser")]
	static void Init()
    {
        ItemBrowserWindow window = GetWindow<ItemBrowserWindow>() as ItemBrowserWindow;
        window.Show();
    }
}
