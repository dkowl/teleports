using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIAnimatorCommands
{
    public class SetTriggerCommand : TimedCommand
    {
        private readonly string skipTriggerName = "Skip";

        private Animator animator;
        private string triggerName;

        private bool isSkipped;

        public SetTriggerCommand(Animator animator, string triggerName, AnimationClip clipToPlay) : 
            base(clipToPlay.length)
        {
            Init(animator, triggerName);
        }

        public SetTriggerCommand(Animator animator, string triggerName, float finishTime) : 
            base(finishTime)
        {
            Init(animator, triggerName);
        }

        private void Init(Animator animator, string triggerName)
        {
            Debug.Assert(animator != null);
            Debug.Assert(animator.HasTrigger(triggerName));
            Debug.Assert(animator.HasTrigger(skipTriggerName));
            
            this.animator = animator;
            this.triggerName = triggerName;
            isSkipped = false;
        }

        public override void Execute()
        {
            animator.SetTrigger(triggerName);
        }

        public override void Skip()
        {
            animator.SetTrigger(skipTriggerName);
            isSkipped = true;
        }

        public override bool IsFinished()
        {
            return base.IsFinished() || isSkipped;
        }
    }
}
