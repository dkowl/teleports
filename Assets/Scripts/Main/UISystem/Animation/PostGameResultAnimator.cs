using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UIAnimatorCommands;
using UnityEngine;
using MultiCommand = UIAnimatorCommands.MultiCommand;

public class PostGameResultAnimator : MonoBehaviour
{

    [System.Serializable]
    private class AnimatedProgressBar
    {
        [SerializeField] private BasicProgressBar progressBar;
        [SerializeField] private AnimationClip loadStartClip;
        [SerializeField] private AnimationClip loadFinishClip;
        [SerializeField] private Animator animator;
        [SerializeField] private string loadStartTriggerName = "LoadStart";
        [SerializeField] private string loadFinishTriggerName = "LoadFinish";
        [SerializeField] private string showDeltaTriggerName = "ShowDelta";
        [SerializeField] private string hideDeltaTriggerName = "HideDelta";

        public bool IsCorrectlyInitialized()
        {
            return
                progressBar != null &&
                loadStartClip != null &&
                loadFinishClip != null &&
                animator != null &&
                animator.HasTrigger(loadStartTriggerName) &&
                animator.HasTrigger(loadFinishTriggerName) &&
                animator.HasTrigger(showDeltaTriggerName) &&
                animator.HasTrigger(hideDeltaTriggerName);
        }

        public IUIAnimatorCommand LoadStartCommand()
        {
            return new SetTriggerCommand(animator, loadStartTriggerName, loadStartClip);
        }

        public IUIAnimatorCommand LoadFinishCommand()
        {
            return new SetTriggerCommand(animator, loadFinishTriggerName, loadFinishClip);
        }

        public IUIAnimatorCommand ShowDeltaCommand()
        {
            return new SetTriggerCommand(animator, showDeltaTriggerName, 0);
        }

        public IUIAnimatorCommand HideDeltaCommand()
        {
            return new SetTriggerCommand(animator, hideDeltaTriggerName, 0);
        }

        public IUIAnimatorCommand SetValuesCommand(BasicProgressBar.Values values)
        {
            return new ChangeProgressBarValuesCommand(progressBar, values);
        }

        public IUIAnimatorCommand InterpretAndSetValuesCommand(float current, float target)
        {
            return new ChangeProgressBarValuesCommand(progressBar, progressBar.InterpretValues(current, target));
        }
    }

    [SerializeField] private AnimatedProgressBar xpProgressBar;
    [SerializeField] private AnimatedProgressBar rpProgressBar;
    private IUIAnimatorCommand currentCommand;
    private List<IUIAnimatorCommand> commands;

    private void Awake()
    {
        Debug.Assert(xpProgressBar != null);
        Debug.Assert(xpProgressBar.IsCorrectlyInitialized());
        Debug.Assert(rpProgressBar != null);
        Debug.Assert(rpProgressBar.IsCorrectlyInitialized());
        commands = new List<IUIAnimatorCommand>();
    }

    private void Update()
    {
        if (currentCommand == null && commands.Count > 0)
        {
            StartNextCommand();
        }

        if (currentCommand == null) return;

        currentCommand.Update(Time.deltaTime);

        if (currentCommand.IsFinished())
        {
            FinishCurrentCommand();
        }
    }

    private void OnDisable()
    {
        // TODO: Skip all commands
    }

    private void AddCommand(IUIAnimatorCommand command)
    {
        commands.Add(command);
    }

    private void StartNextCommand()
    {
        Debug.Assert(currentCommand == null);
        Debug.Assert(commands.Count > 0);
        currentCommand = commands.First();
        currentCommand.Execute();
    }

    private void SkipCurrentCommand()
    {
        currentCommand.Skip();
    }

    private void FinishCurrentCommand()
    {
        commands.RemoveAt(0);
        currentCommand = null;
    }

    public void HandlePostGamePopUpEvents(IEnumerable<PostGamePopUpEvent> popUpEvents)
    {
        foreach (var popUpEvent in popUpEvents)
        {
            switch (popUpEvent.Type)
            {
                case PostGamePopUpEventType.XpEarned:
                    HandleXpEarned((PostGamePopUpEvent_XpEarned)popUpEvent);
                    break;
                case PostGamePopUpEventType.RpEarned:
                    HandleRpEarned((PostGamePopUpEvent_RpEarned)popUpEvent);
                    break;
            }
        }
    }

    public void HandleXpEarned(PostGamePopUpEvent_XpEarned xpEarnedEvent)
    {
        if (xpEarnedEvent.IsStartingASequence)
        {
            AddCommand(xpProgressBar.LoadStartCommand());
        }

        AddCommand(
            new UIAnimatorCommands.MultiCommand(
                xpProgressBar.InterpretAndSetValuesCommand(xpEarnedEvent.OldXp, xpEarnedEvent.NewXp),
                xpProgressBar.ShowDeltaCommand()
            )
        );
        AddCommand(xpProgressBar.HideDeltaCommand());

        if (xpEarnedEvent.IsEndingASequence)
        {
            AddCommand(xpProgressBar.LoadFinishCommand());
        }
    }

    public void HandleRpEarned(PostGamePopUpEvent_RpEarned rpEarnedEvent)
    {
        if (rpEarnedEvent.IsStartingASequence)
        {
            AddCommand(rpProgressBar.LoadStartCommand());
        }

        AddCommand(
            new UIAnimatorCommands.MultiCommand(
                rpProgressBar.InterpretAndSetValuesCommand(rpEarnedEvent.OldRp, rpEarnedEvent.NewRp),
                rpProgressBar.ShowDeltaCommand()
            )
        );
        AddCommand(rpProgressBar.HideDeltaCommand());

        if (rpEarnedEvent.IsEndingASequence)
        {
            AddCommand(rpProgressBar.LoadFinishCommand());
        }
    }

    [Button]
    private void TestRun()
    {
        var test = new Test_GeneratePostGamePopUpEvents();
        var events = test.GenerateEvents();
        HandlePostGamePopUpEvents(events);
    }
}
