using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCommand : Command {

    protected Command[] subCommands;
    protected int finishedCount;

    public MultiCommand(Command[] commands)
    {
        //Debug.Log("Creating MultiCommand from " + commands.Length.ToString() + " commands");
        subCommands = commands;
        finishedCount = 0;
    }

    protected override void StartInternal()
    {
        if (subCommands.Length == 0)
        {
            Finish();
        }
        foreach (var subCommand in subCommands)
        {
            subCommand.RegisterFinishCallback(OnSubCommandFinished);
            subCommand.Start();
        }
    }

    public void OnSubCommandFinished()
    {
        finishedCount++;
        if(finishedCount == subCommands.Length)
        {
            Finish();
        }
    }

    protected override void FinishInternal()
    {
        
    }
}
