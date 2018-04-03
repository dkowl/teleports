using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CheatCommand
{
    private string name;
    private Action<string[]> action;

    public CheatCommand(string name, Action<string[]> action)
    {
        this.name = name;
        this.action = action;
    }

    public bool IsValidInput(string input)
    {
        return input.StartsWith(name + " ");
    }

    public bool ProcessInput(string input)
    {
        if (!IsValidInput(input)) return false;
        string[] args = input.Split(' ');
        action(args);
        return true;
    }
}
