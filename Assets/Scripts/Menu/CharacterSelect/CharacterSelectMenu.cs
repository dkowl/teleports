using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CharacterSelectMenu : MonoBehaviour {

    public string selectCharacterString, newCharacterString;
    public Text titleText;
    public Text playerNameText;
    public Text playerLevelText;

    void OnEnable()
    {
        if (Application.isEditor)
        {
            LoadData();
        }
    }

    void LoadData()
    {
        Stylesheet stylesheet = MainData.CurrentStylesheet;

        titleText.text = selectCharacterString;
        playerNameText.text = MainData.CurrentPlayerData.CharacterName;
        playerLevelText.text = "Level " + MainData.CurrentPlayerData.Level.ToString();
    }
}
