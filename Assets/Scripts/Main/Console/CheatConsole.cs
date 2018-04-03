using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class CheatConsole : MonoBehaviour
{

    [SerializeField] private GameObject consoleObject;
    [SerializeField] private TMPro.TMP_InputField inputField;
    [SerializeField] private Text outputText;
    private bool isEnabled;
    private string outputString;


    private void Awake()
    {
        Disable();
        outputString = "";
        inputField.onFocusSelectAll = false;
        UpdateOutputText();
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
        Output(">> " + inputField.text);
        inputField.text = "";
        inputField.ActivateInputField();
    }

    private void Output(string text)
    {
        text += "\n";
        outputString += text;
        UpdateOutputText();
    }

    private void UpdateOutputText()
    {
        outputText.text = outputString;
    }

}
