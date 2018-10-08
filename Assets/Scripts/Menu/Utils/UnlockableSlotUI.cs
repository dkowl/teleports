using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UnlockableSlotUI : LoadableBehaviour {
    
    public Image foregroundImage, backgroundImage;
    public Text text;
        
    [SerializeField] protected Stylesheet.ColorPreset lockedColor;
    [SerializeField] protected Stylesheet.ColorPreset unlockedColor;
    [SerializeField] protected Stylesheet.ColorPreset lockColor;

    protected Stylesheet stylesheet;
    protected UnlockableSlot slot;

    protected override void LoadDataInternal()
    {
        stylesheet = Main.StaticData.Stylesheet;
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
        backgroundImage.color = stylesheet.GetColorPreset(lockedColor);
        foregroundImage.sprite = stylesheet.lockSprite;
        foregroundImage.color = stylesheet.GetColorPreset(lockColor);
        text.text = "";        
    }

    protected virtual void OnEmpty()
    {
        backgroundImage.color = stylesheet.GetColorPreset(unlockedColor);
        foregroundImage.color = Color.clear;
        text.text = "";
    }

    protected virtual void OnFull()
    {
        backgroundImage.color = stylesheet.GetColorPreset(unlockedColor);
        foregroundImage.color = Color.clear;
        text.text = "Full";
    }
}
