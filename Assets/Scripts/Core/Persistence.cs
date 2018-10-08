using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Persistence : MonoBehaviour, IPersistence
{

    [SerializeField] private GameDataSO gameDataSo;
    [SerializeField] private GraphicsDataSO graphicsDataSo;
    [SerializeField] private ServerDataSO serverDataSo;
    [SerializeField] private Stylesheet stylesheet;
    [SerializeField] private DataDefaults defaults;

    public IStaticData GetStaticData()
    {
        Debug.Assert(!gameDataSo.Empty, "GameData not found");
        Debug.Assert(!graphicsDataSo.Empty, "GraphicsData not found");
        Debug.Assert(stylesheet != null, "Stylesheet not found");
        Debug.Assert(defaults != null, "DataDefaults not found");

        StaticData result = new StaticData(gameDataSo.Data, graphicsDataSo.Data, stylesheet, defaults);
        return result;
    }

    public IGameState LoadGameState()
    {
        return new GameState();
    }

    public IServerData GetServerData()
    {
        Debug.Assert(!serverDataSo.Empty, "ServerData not found");

        return serverDataSo.Data;
    }
}
