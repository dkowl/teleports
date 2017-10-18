using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCommand : Command {

    protected Command[] subCommands;
    protected int finishedCount;

    public MultiCommand(Command[] commands)
    {
        subCommands = commands;
        finishedCount = 0;
    }

    protected override void StartInternal()
    {
        foreach(var subCommand in subCommands)
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
