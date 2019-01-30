using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterThumbnailUI : SelectorButtonUI {

    public Image playerIcon, teleportIcon, newHeroIcon;
    public Text characterName, characterLvl;

    private int characterSlotID;
    private HeroData heroData;
    private CreateHeroConfirmDWSB createHeroConfirmDWSB;

    protected override void LoadDataInternal()
    {
        base.LoadDataInternal();

        heroData = Main.GameState.GetHeroData(characterSlotID);
        if (createHeroConfirmDWSB == null)
        {
            createHeroConfirmDWSB = gameObject.AddComponent<CreateHeroConfirmDWSB>();
        }

        if (heroData != null)
        {
            playerIcon.gameObject.SetActive(true);
            teleportIcon.gameObject.SetActive(true);
            newHeroIcon.gameObject.SetActive(false);

            playerIcon.sprite = PlayerGraphics.GetPlayerIcon(heroData);
            teleportIcon.sprite = PlayerGraphics.GetTeleportIcon(heroData);

            characterName.text = heroData.CharacterName;
            characterLvl.text = "Lvl " + heroData.Level.ToString();
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
        return characterSlotID == Main.GameState.CurrentHeroId;
    }

    protected override void OnActivate()
    {
        if(Main.GameState.GetHeroData(characterSlotID) != null) 
            Main.GameState.SelectHero(characterSlotID);
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

    public int CharacterSlotID => characterSlotID;
}
