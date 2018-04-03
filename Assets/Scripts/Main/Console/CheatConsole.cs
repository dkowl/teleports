using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class CheatConsole : Singleton<CheatConsole>
{

    [SerializeField] private GameObject consoleObject;
    [SerializeField] private TMPro.TMP_InputField inputField;
    [SerializeField] private Text outputText;
    private bool isEnabled;
    private string outputString;
    private List<CheatCommand> commands;


    private void Awake()
    {
        Disable();
        outputString = "";
        inputField.onFocusSelectAll = false;
        UpdateOutputText();

        commands = new List<CheatCommand>
        {
            new CheatCommand("AddItem", CheatActions.AddItem)
        };
    }

    private void Update()
    {
        if (Input.GetButtonDown("CheatConsole"))
        {
            if(isEnabled) Disable();
            else Enable();
        }


        if (Input.GetKeyDown("return"))
        {
            Submit();
        }
    }

    public void Output(string text)
    {
        text += "\n";
        outputString += text;
        UpdateOutputText();
    }

    private void Enable()
    {
        isEnabled = true;
        consoleObject.SetActive(true);
        inputField.ActivateInputField();
    }

    private void Disable()
    {
        isEnabled = false;
        consoleObject.SetActive(false);
    }

    private void Submit()
    {
        string input = inputField.text;
        Output(">> " + input);
        ProcessInput(input);
        inputField.text = "";
        inputField.ActivateInputField();
    }

    private void UpdateOutputText()
    {
        outputText.text = outputString;
    }

    private void ProcessInput(string input)
    {
        foreach (var command in commands)
        {
            if (command.ProcessInput(input)) return;
        }

        Output($"command \"{input}\" not found");
    }
}
