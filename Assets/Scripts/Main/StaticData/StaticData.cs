using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StaticData : IStaticData {

    private GameData gameData;
    private GraphicsData graphicsData;
    private UIData uiData;
    private DataDefaults defaults;

    public StaticData(
        GameData gameData, 
        GraphicsData graphicsData, 
        UIData uiData, 
        DataDefaults defaults)
    {
        this.gameData = gameData;
        this.graphicsData = graphicsData;
        this.uiData = uiData;
        this.defaults = defaults;
    }

    public IGameData Game => gameData;
    public IGraphicsData Graphics => graphicsData;
    public UIData UI => uiData;
    public Stylesheet Stylesheet => uiData.Stylesheet;
    public DataDefaults Defaults => defaults;
}
