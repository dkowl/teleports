using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Teleports.Utils;

public class ButtonSpawner : PrefabSpawner {

    public delegate void OnClickCallback();

    protected string textString;
    protected OnClickCallback callback;
    protected RectTransform spawnTransform;

    protected override void AfterSpawn()
    {

        Button button = SpawnedInstance.GetComponent<Button>();
        Text text = null;
        button.FindOrSpawnChildWithComponent(ref text, "Text");
        text.text = textString;
        button.onClick.AddListener(Invoke);
    }

    public void Invoke()
    {
        callback();
    }

    public string TextString
    {
        set { textString = value; }
    }

    public OnClickCallback Callback
    {
        set { callback = value; }
    }
}
