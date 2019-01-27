using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIAnimatorCommands
{
    public class ChangeProgressBarValuesCommand : TimedCommand
    {
        private BasicProgressBar bar;
        private BasicProgressBar.Values values;

        public ChangeProgressBarValuesCommand(BasicProgressBar bar, BasicProgressBar.Values values) :
            base(0.5f)
        {
            this.bar = bar;
            this.values = values;
        }

        public override void Execute()
        {
            bar.SetValues(values);
        }

        public override void Skip()
        {
            bar.Skip();
        }

        public override bool IsFinished()
        {
            return !bar.IsAnimating && base.IsFinished();
        }
    }
}
