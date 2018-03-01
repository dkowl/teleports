using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterThumbnailUI : SelectorButtonUI {

    public Image playerIcon, teleportIcon, newHeroIcon;
    public Text characterName, characterLvl;

    private int characterSlotID;
    private PlayerData playerData;
    private CreateHeroConfirmDWSB createHeroConfirmDWSB;

    override protected void LoadDataInternal()
    {
        base.LoadDataInternal();

        playerData = MainData.Save.GetPlayerData(characterSlotID);
        if (createHeroConfirmDWSB == null)
        {
            createHeroConfirmDWSB = gameObject.AddComponent<CreateHeroConfirmDWSB>();
        }

        if (playerData != null)
        {
            playerIcon.gameObject.SetActive(true);
            teleportIcon.gameObject.SetActive(true);
            newHeroIcon.gameObject.SetActive(false);

            playerIcon.sprite = PlayerGraphics.GetPlayerIcon(playerData);
            teleportIcon.sprite = PlayerGraphics.GetTeleportIcon(playerData);

            characterName.text = playerData.CharacterName;
            characterLvl.text = "Lvl " + playerData.Level.ToString();
        }
        else
        {
            playerIcon.gameObject.SetActive(false);
            teleportIcon.gameObject.SetActive(false);
            newHeroIcon.gameObject.SetActive(true);

            characterName.text = "Empty";
            characterLvl.text = "";
        }
    }

    protected override bool IsActive()
    {
        return characterSlotID == MainData.Save.CurrentSlotID;
    }

    protected override void OnActivate()
    {
        if(MainData.Save.GetPlayerData(characterSlotID) != null) 
            MainData.Save.CurrentSlotID = characterSlotID;
    }

    protected override void OnDeactivate()
    {
        
    }

    public void SetCharacterSlotID(int id)
    {
        characterSlotID = id;
        createHeroConfirmDWSB.CharacterSlotId = id;
        LoadDataInternal();
    }

    public int CharacterSlotID
    {
        get { return characterSlotID; }
    }
}
