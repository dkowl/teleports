using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PlayerName : MonoBehaviour {

	void Start () {
        Text text = gameObject.GetComponent<Text>();
        text.text = MainData.PlayerName;
	}
}
