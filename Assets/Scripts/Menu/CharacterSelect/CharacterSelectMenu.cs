using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CharacterSelectMenu : LoadableBehaviour {

    public string selectCharacterString, newCharacterString;
    public Text titleText;
    public Text playerNameText;
    public Text playerLevelText;

    public override void LoadDataInternal()
    {
        Stylesheet stylesheet = MainData.CurrentStylesheet;

        titleText.text = selectCharacterString;
        playerNameText.text = MainData.CurrentPlayerData.CharacterName;
        playerLevelText.text = "Level " + MainData.CurrentPlayerData.Level.ToString();
    }
}
