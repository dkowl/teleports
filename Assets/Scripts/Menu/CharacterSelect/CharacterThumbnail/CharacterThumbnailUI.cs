using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterThumbnailUI : LoadableBehaviour {

    public Image playerIcon, teleportIcon;
    public Text characterName, characterLvl;
    public float inactiveScaleFactor = 0.8f;

    private int characterSlotID = 0;
    private IPlayerData playerData = null;

    public override void LoadDataInternal()
    {
        if (playerData != null)
        {
            playerIcon.sprite = PlayerGraphics.GetPlayerIcon(playerData);
            teleportIcon.sprite = PlayerGraphics.GetTeleportIcon(playerData);

            characterName.text = playerData.CharacterName;
            characterLvl.text = "Lvl " + playerData.Level.ToString();

            if(characterSlotID == MainData.CurrentSaveData.CurrentSlotID)
            {
                SetActive();
            }
            else
            {
                SetInactive();
            }
        }
    }

    public void SetCharacterSlotID(int id)
    {
        characterSlotID = id;
        playerData = MainData.CurrentSaveData.GetPlayerData(characterSlotID);
        LoadDataInternal();
    }

    public int CharacterSlotID
    {
        get { return characterSlotID; }
    }

    public void SetActive()
    {
        transform.localScale = Vector3.one;
        if (characterSlotID != MainData.CurrentSaveData.CurrentSlotID)
        {
            MainData.CurrentSaveData.CurrentSlotID = characterSlotID;
        }
    }

    public void SetInactive()
    {
        transform.localScale = Vector3.one * inactiveScaleFactor;
    }
}
