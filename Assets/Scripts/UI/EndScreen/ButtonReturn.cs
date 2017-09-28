using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonReturn : MonoBehaviour {

	void Awake () {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(GameMain.Instance.BackToHome);
    }
}
