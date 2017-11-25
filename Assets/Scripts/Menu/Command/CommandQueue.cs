using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandQueue {

    public enum AddMode
    {
        Standard,
        Priority,
        Instant
    }

    private static readonly bool debugMode = false;
    private string debugString = "command";

    protected LinkedList<Command> commands = new LinkedList<Command>();

    public void Update()
    {
        if (commands.Count == 0)
        {
            return;
        }

        Command currentCommand = commands.First.Value;

        DebugStringUpdate(currentCommand);

        if(currentCommand.State == CommandState.Pending)
        {
            if(debugMode) Debug.Log("Starting " + debugString + ", count: " + commands.Count.ToString());
            currentCommand.Start();
        }
        else if(currentCommand.State == CommandState.Finished)
        {
            if (debugMode) Debug.Log("Removing " + debugString + ", count: " + commands.Count.ToString());
            commands.RemoveFirst();
            Update();
        }
    }

    public void AddCommand(Command command, AddMode addMode = AddMode.Standard)
    {
        DebugStringUpdate(command);
        if (debugMode) Debug.Log("Adding " + debugString + ", count: " + commands.Count.ToString());
        command.RegisterFinishCallback(Update);

        switch (addMode)
        {
            case AddMode.Standard:
                commands.AddLast(command);
                break;

            case AddMode.Priority:
                LinkedListNode<Command> i = commands.First;
                bool added = false;

                while(i.Value.State != CommandState.Pending)
                {
                    if(i == commands.Last)
                    {
                        commands.AddLast(command);
                        added = true;
                        break;
                    }
                    i = i.Next;
                }
                if (!added)
                {
                    commands.AddBefore(i, command);
                }
                break;

            case AddMode.Instant:
                commands.AddFirst(command);
                break;
        }

        Update();
    }

    //Debug Methods
    private void DebugStringUpdate(Command command)
    {
        if (!debugMode) return;
        if (command is MenuCommand) debugString = "MenuCommand";
        else if (command is MenuBehaviourCommand) debugString = "MenuBehaviourCommand";
        else if (command is MultiCommand) debugString = "MultiCommand";
        else debugString = "Command";
    }

    public void DebugPrintQueue()
    {
        foreach(var command in commands)
        {
            Debug.Log(command.GetType().ToString());
        }
    }
}
