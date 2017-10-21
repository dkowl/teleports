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

    protected LinkedList<Command> commands = new LinkedList<Command>();

    public void Update()
    {
        Debug.Log("Updating Queue, command count: " + commands.Count.ToString());
        if (commands.Count == 0)
        {
            return;
        }

        Command currentCommand = commands.First.Value;

        if(currentCommand.State == CommandState.Pending)
        {
            currentCommand.Start();
            Debug.Log("Starting command, count: " + commands.Count.ToString());
        }
        else if(currentCommand.State == CommandState.Finished)
        {
            Debug.Log("Removing command, command count: " + commands.Count.ToString());
            commands.RemoveFirst();
            Update();
        }
    }

    public void AddCommand(Command command, AddMode addMode = AddMode.Standard)
    {
        Debug.Log("Adding command, command count: " + commands.Count.ToString());
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
