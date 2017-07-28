using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour {

    public string selectCharacterString, newCharacterString;
    public Text titleText;

    void Start()
    {
        titleText.fontSize = MainData.CurrentStylesheet.titleTextSize;
        titleText.text = selectCharacterString;
    }

}
