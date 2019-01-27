using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

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

        public bool IsCorrectlyInitialized()
        {
            return
                progressBar != null &&
                loadStartClip != null &&
                loadFinishClip != null &&
                animator != null &&
                animator.HasTrigger(loadStartTriggerName) &&
                animator.HasTrigger(loadFinishTriggerName) &&
                animator.HasTrigger(showDeltaTriggerName);
        }

        public void LoadStart()
        {
            animator.SetTrigger(loadStartTriggerName);
        }

        public void LoadFinish()
        {
            animator.SetTrigger(loadFinishTriggerName);
        }

        public void ShowDelta()
        {
            animator.SetTrigger(showDeltaTriggerName);
        }

        public void SetValues(BasicProgressBar.Values values)
        {
            progressBar.SetValues(values);
        }

        public AnimationClip LoadStartClip => loadStartClip;
        public AnimationClip LoadFinishClip => loadFinishClip;
    }

    private enum CommandType
    {
        ChangeSliderValues,
        StartSequence,
        EndSequence
    }

    private enum SliderType
    {
        XP,
        RP
    }

    private class Command
    {
        public CommandType type;
        public SliderType sliderType;
        public bool waitForClick;
        public float totalTime;
        public BasicProgressBar.Values sliderValues;
    }
    

    [SerializeField] private AnimatedProgressBar xpProgressBar;
    [SerializeField] private AnimatedProgressBar rpProgressBar;
    [SerializeField] private float defaultCommandTime = 2.0f;
    private float currentCommandFinishTime;
    private float currentCommandRunTime;
    private Command currentCommand;
    private List<Command> commands;

    private void Awake()
    {
        Debug.Assert(xpProgressBar != null);
        Debug.Assert(xpProgressBar.IsCorrectlyInitialized());
        Debug.Assert(rpProgressBar != null);
        Debug.Assert(rpProgressBar.IsCorrectlyInitialized());
        commands = new List<Command>();
    }

    private void Update()
    {
        if (currentCommand == null && commands.Count > 0)
        {
            StartNextCommand();
        }

        if (currentCommand == null) return;

        currentCommandRunTime += Time.deltaTime;

        if (currentCommandRunTime >= currentCommandFinishTime)
        {
            FinishCurrentCommand();
        }
    }

    private void OnDisable()
    {
        // TODO: Skip all commands
    }

    private void AddCommand(Command command)
    {
        commands.Add(command);
    }

    private void StartNextCommand()
    {
        Debug.Assert(currentCommand == null);
        Debug.Assert(commands.Count > 0);
        currentCommand = commands.First();
        currentCommandFinishTime = currentCommand.waitForClick ? 1000000 : currentCommand.totalTime;
        currentCommandRunTime = 0;
        ExecuteCurrentCommand();
    }

    private void ExecuteCurrentCommand()
    {
        var progressBar = GetProgressBarByType(currentCommand.sliderType);

        switch (currentCommand.type)
        {
            case CommandType.StartSequence:
                progressBar.LoadStart();
                break;
            case CommandType.ChangeSliderValues:
                progressBar.SetValues(currentCommand.sliderValues);
                progressBar.ShowDelta();
                break;
            case CommandType.EndSequence:
                progressBar.LoadFinish();
                break;
        }
    }

    private void SkipCurrentCommand()
    {
        
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
        const SliderType sliderType = SliderType.XP;

        if (xpEarnedEvent.IsStartingASequence)
        {
            AddSequenceCommand(sliderType, CommandType.StartSequence);
        }

        AddCommand(new Command()
        {
            sliderType = sliderType,
            type = CommandType.ChangeSliderValues,
            totalTime = defaultCommandTime,
            sliderValues = new BasicProgressBar.Values()
            {
                current = Levels.xp.AboveCurrentLevel(xpEarnedEvent.OldXp),
                target = Levels.xp.AboveCurrentLevel(xpEarnedEvent.OldXp) + xpEarnedEvent.NewXp - xpEarnedEvent.OldXp,
                delta = xpEarnedEvent.NewXp - xpEarnedEvent.OldXp,
                min = 0,
                max = Levels.xp.RequiredFromCurrentLevelToNext(xpEarnedEvent.OldXp)
            }
        });

        if (xpEarnedEvent.IsEndingASequence)
        {
            AddSequenceCommand(sliderType, CommandType.EndSequence);
        }
    }

    public void HandleRpEarned(PostGamePopUpEvent_RpEarned rpEarnedEvent)
    {
        const SliderType sliderType = SliderType.RP;

        if (rpEarnedEvent.IsStartingASequence)
        {
            AddSequenceCommand(sliderType, CommandType.StartSequence);
        }

        AddCommand(new Command()
        {
            sliderType = sliderType,
            type = CommandType.ChangeSliderValues,
            totalTime = defaultCommandTime,
            sliderValues = new BasicProgressBar.Values()
            {
                current = rpEarnedEvent.OldRp,
                target = rpEarnedEvent.NewRp,
                delta = rpEarnedEvent.NewRp - rpEarnedEvent.OldRp,
                min = Levels.rp.RequiredTotalForCurrentLevel(rpEarnedEvent.OldRp),
                max = Levels.rp.RequiredTotalForNextLevel(rpEarnedEvent.OldRp)
            }
        });

        if (rpEarnedEvent.IsEndingASequence)
        {
            AddSequenceCommand(sliderType, CommandType.EndSequence);
        }
    }

    private void AddSequenceCommand(SliderType sliderType, CommandType commandType)
    {
        Debug.Assert(commandType == CommandType.StartSequence || commandType == CommandType.EndSequence);

        var progressBar = GetProgressBarByType(sliderType);
        var clip = commandType == CommandType.StartSequence ? progressBar.LoadStartClip : progressBar.LoadFinishClip;

        AddCommand( new Command()
        {
            sliderType = sliderType,
            type = commandType,
            totalTime = clip.length
        });
    }

    private AnimatedProgressBar GetProgressBarByType(SliderType type)
    {
        switch (type)
        {
            case SliderType.XP:
                return xpProgressBar;
            case SliderType.RP:
                return rpProgressBar;
        }

        return null;
    }

    [Button]
    private void TestRun()
    {
        var test = new Test_GeneratePostGamePopUpEvents();
        var events = test.GenerateEvents();
        HandlePostGamePopUpEvents(events);
    }
}
