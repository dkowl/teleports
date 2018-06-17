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
        StaticData result = new StaticData(gameDataSo.Data, graphicsDataSo.Data, serverDataSo.Data, stylesheet, defaults);
        return result;
    }
}
