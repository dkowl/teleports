using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Teleports.Utils;

public class ButtonSpawner : PrefabSpawner {

    public delegate void OnClickCallback();

    [SerializeField] protected RectTransform spawnTransform;
    protected List<ButtonChoice> choices;

    protected override void AfterSpawn()
    {
        if (choices != null && currentId < choices.Count)
        {
            SpawnedInstance.transform.SetParent(spawnTransform);

            Button button = SpawnedInstance.GetComponent<Button>();

            Text text = null;
            button.FindOrSpawnChildWithComponent(ref text, "Text");
            text.text = choices[currentId].Text;

            button.onClick.AddListener(choices[currentId].InvokeCallback);
            button.onClick.AddListener(MenuController.Instance.CloseTopMenu);
        }
    }

    public List<ButtonChoice> Choices
    {
        set { choices = value; }
    }

}
