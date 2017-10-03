using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenUI : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI text;

    public void SetText(string textString)
    {
        text.text = textString;
    }
}
