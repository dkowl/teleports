using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using FullSerializer;


public class Persistence : MonoBehaviour, IPersistence
{
    [SerializeField] private GameDataSO gameDataSo;
    [SerializeField] private GraphicsDataSO graphicsDataSo;
    [SerializeField] private Stylesheet stylesheet;
    [SerializeField] private DataDefaults defaults;

    [SerializeField] private ServerDataSO serverDataSo;

    private GameState gameState;
    private IPersistor<GameState> gameStatePersistor = new JsonGameStatePersistor();

    public static event System.Action LoadFinishEvent;
    public static event System.Action SaveFinishEvent;

    public IStaticData GetStaticData()
    {
        Debug.Assert(!gameDataSo.Empty, "GameData not found");
        Debug.Assert(!graphicsDataSo.Empty, "GraphicsData not found");
        Debug.Assert(stylesheet != null, "Stylesheet not found");
        Debug.Assert(defaults != null, "DataDefaults not found");

        StaticData result = new StaticData(gameDataSo.Data, graphicsDataSo.Data, stylesheet, defaults);
        return result;
    }

    [Button]
    public void LoadGameState()
    {
        gameState = gameStatePersistor.Load();
        LoadFinishEvent?.Invoke();
    }

    [Button]
    public void SaveGameState()
    {
        gameStatePersistor.Save(gameState);
        SaveFinishEvent?.Invoke();
    }

    public IGameState GetGameState()
    {
        if (gameState == null)
        {
            LoadGameState();
        }

        return gameState;
    }

    public IServerData GetServerData()
    {
        Debug.Assert(!serverDataSo.Empty, "ServerData not found");

        return serverDataSo.Data;
    }
}
