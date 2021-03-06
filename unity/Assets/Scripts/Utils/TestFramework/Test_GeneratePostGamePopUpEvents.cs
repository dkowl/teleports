﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Test_GeneratePostGamePopUpEvents : TestBase {
    
    protected override void RunInternal()
    {
        var result = GenerateEvents();
        Assert(result != null);
        Assert(result[0].Type == PostGamePopUpEventType.XpEarned);
    }

    public List<PostGamePopUpEvent> GenerateEvents()
    {
        Type type = typeof(HeroData);
        MethodInfo methodInfo = type.GetMethod("GeneratePostGamePopUpEvents", BindingFlags.NonPublic | BindingFlags.Static);
        Assert(methodInfo != null);
        var result = methodInfo.Invoke(null, new object[] { 5000, 15000, 3000, 1000 }) as List<PostGamePopUpEvent>;
        return result;
    }

    public override string Name => "GeneratePostGamePopUpEvents";
}
