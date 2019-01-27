using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIAnimatorCommands
{
    public class MultiCommand : IUIAnimatorCommand
    {
        private List<IUIAnimatorCommand> commands;

        public MultiCommand(params IUIAnimatorCommand[] commands)
        {
            this.commands = new List<IUIAnimatorCommand>(commands);
        }

        public void Execute()
        {
            foreach (var command in commands)
            {
                command.Execute();
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var command in commands)
            {
                command.Update(deltaTime);
            }
        }

        public void Skip()
        {
            foreach (var command in commands)
            {
                command.Skip();
            }
        }

        public bool IsFinished()
        {
            foreach (var command in commands)
            {
                if (!command.IsFinished()) return false;
            }

            return true;
        }
    }
}
