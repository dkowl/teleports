using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Text = TMPro.TextMeshProUGUI;

public class DialogWindow : MenuBehaviour {    

    [SerializeField] protected Text text;
    [SerializeField] protected GameObject buttonPrefab;
    [SerializeField] protected ButtonSpawner buttonSpawner;

    protected string textString;
    protected List<ButtonChoice> choices;

    public void Setup(string textString, List<ButtonChoice> choices)
    {
        this.textString = textString;
        this.choices = choices;

        buttonSpawner.SpawnAmount = choices.Count;
        buttonSpawner.Choices = choices;

        text.text = textString;
        buttonSpawner.Respawn();
    }
}
