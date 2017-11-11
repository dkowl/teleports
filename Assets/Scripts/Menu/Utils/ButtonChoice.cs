using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChoice
{
    public delegate void Callback();

    private string text;
    private Callback callback;

    public ButtonChoice(string text, Callback callback)
    {
        this.text = text;
        this.callback = callback;
    }

    public void InvokeCallback()
    {
        callback();
    }

    public string Text
    {
        get { return text; }
    }
}
