using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Levels : TestBase {

    protected override void RunInternal()
    {
        var intervals = Levels.xp.GetSliderProgressionIntervals(5000, 15000);
        Assert(intervals.Count == 3);
        Assert(intervals[0].Item1 == 5000);
        Assert(intervals[0].Item2 == 6990);
        Assert(intervals[1].Item1 == 6990);
        Assert(intervals[1].Item2 == 11340);
        Assert(intervals[2].Item1 == 11340);
        Assert(intervals[2].Item2 == 15000);
    }

    public override string Name => "Test_Levels";
}
