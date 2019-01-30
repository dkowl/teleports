using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedCommand : IUIAnimatorCommand
{
    private float runTime;
    private float finishTime;

    public TimedCommand(float time)
    {
        runTime = 0;
        finishTime = time;
    }

    public virtual void Execute()
    {

    }

    public virtual void Update(float deltaTime)
    {
        runTime += deltaTime;
    }

    public virtual void Skip()
    {
        runTime = finishTime;
    }

    public virtual bool IsFinished()
    {
        return runTime >= finishTime;
    }
}
