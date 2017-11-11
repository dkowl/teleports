using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Text = TMPro.TextMeshProUGUI;

public class DialogWindow : MenuBehaviour {

    public class Choice
    {
        public delegate void Callback();

        private string text;
        private Callback callback;

        public Choice(string text, Callback callback)
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

    [SerializeField] protected Text text;
    [SerializeField] protected RectTransform buttonSpawnTransform;
    [SerializeField] protected GameObject buttonPrefab;

    protected string textString;
    protected List<Choice> choices;
}
