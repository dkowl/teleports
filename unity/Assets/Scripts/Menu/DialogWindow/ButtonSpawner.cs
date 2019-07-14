using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Teleports.Utils;

public class ButtonSpawner : PrefabSpawner {

    public delegate void OnClickCallback();
    
    protected List<ButtonChoice> choices;

    protected override void AfterSpawn()
    {
        if (choices != null && CurrentInstanceId < choices.Count)
        {
            Button button = SpawnedInstance.GetComponent<Button>();

            Text text = null;
            button.FindOrSpawnChildWithComponent(ref text, "Text");
            text.text = choices[CurrentInstanceId].Text;

            button.onClick.AddListener(MenuController.Instance.CloseTopMenu);
            button.onClick.AddListener(choices[CurrentInstanceId].InvokeCallback);
        }
    }

    public List<ButtonChoice> Choices
    {
        set { choices = value; }
    }

}
