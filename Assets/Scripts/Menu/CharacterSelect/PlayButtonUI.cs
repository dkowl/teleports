using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonUI : MonoBehaviour {

    private Button button;

	void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        MenuController.Instance.HideAll();
        MainController.SwitchScene("World");
    }
}
