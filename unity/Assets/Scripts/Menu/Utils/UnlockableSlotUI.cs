using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UnlockableSlotUI : LoadableBehaviour {
    
    public Image foregroundImage, backgroundImage;
    public Text text;
    
    [SerializeField] protected StylesheetKeys.Color lockedColorPreset;
    [SerializeField] protected StylesheetKeys.Color unlockedColorPreset;
    [SerializeField] protected StylesheetKeys.Color lockColorPreset;
    private StylesheetKeys.Sprite lockSpritePreset = new StylesheetKeys.Sprite();
    
    protected IStylesheet stylesheet;
    protected UnlockableSlot slot;

    protected override void LoadDataInternal()
    {
        stylesheet = Main.StaticData.UI.Stylesheet;
        lockSpritePreset.String = "Lock";
        slot = GetSlot();

        if (slot.IsLocked)
        {
            OnLocked();
        }
        else if (slot.IsEmpty)
        {
            OnEmpty();
        }
        else
        {
            OnFull();
        }
    }

    protected abstract UnlockableSlot GetSlot();

    protected virtual void OnLocked()
    {
        backgroundImage.color = stylesheet.GetValue<Color>(lockedColorPreset);
        foregroundImage.sprite = stylesheet.GetValue<Sprite>(lockSpritePreset);
        foregroundImage.color = stylesheet.GetValue<Color>(lockColorPreset);
        text.text = "";        
    }

    protected virtual void OnEmpty()
    {
        backgroundImage.color = stylesheet.GetValue<Color>(unlockedColorPreset);
        foregroundImage.color = Color.clear;
        text.text = "";
    }

    protected virtual void OnFull()
    {
        backgroundImage.color = stylesheet.GetValue<Color>(unlockedColorPreset);
        foregroundImage.color = Color.clear;
        text.text = "Full";
    }
}
