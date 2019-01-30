using System.Collections;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class TestBase : ITest
{
    private bool result;

    public bool Run()
    {
        result = true;
        RunInternal();
        return result;
    }

    protected abstract void RunInternal();

    protected void Assert(bool statement)
    {
        if (!statement) result = false;
        Debug.Assert(statement);
    }

    public virtual string Name => "Unnamed Test";
}
