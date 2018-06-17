using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaticData
{

    IGameData Game { get; }
    IGraphicsData Graphics { get; }
    IServerData Server { get; }
    Stylesheet Stylesheet { get; }
    DataDefaults Defaults { get; }
}
