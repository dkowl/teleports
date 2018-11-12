using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TestFramework : Singleton<TestFramework>, ISingletonInstance
{
    private List<ITest> tests;

    public void OnFirstAccess()
    {
        InitTests();
    }

    private void RunTest(ITest test)
    {
        string resultString = test.Run() ? "Success" : "Fail";
        Debug.Log($"Test {test.Name}: {resultString}");
    }

    [Button]
    private void RunTests()
    {
        InitTests();
        foreach (var test in tests)
        {
            RunTest(test);
        }
    }

    private void InitTests()
    {
        tests = new List<ITest>
        {
            new Test_Levels()
        };
    }


}
