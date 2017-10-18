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

    protected LinkedList<Command> commands;

    public void Update()
    {
        if(commands.Count == 0)
        {
            return;
        }

        Command currentCommand = commands.First.Value;

        if(currentCommand.State == CommandState.Pending)
        {
            currentCommand.Start();
        }
        else if(currentCommand.State == CommandState.Finished)
        {
            commands.RemoveFirst();
        }
    }

    public void AddCommand(Command command, AddMode addMode = AddMode.Standard)
    {
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
}
