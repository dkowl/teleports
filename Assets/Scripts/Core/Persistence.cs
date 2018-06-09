using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Persistence : MonoBehaviour, IPersistence
{

    [SerializeField] private GameDataSO gameDataSo;
    [SerializeField] private GraphicsDataSO graphicsDataSo;

    public StaticData GetStaticData()
    {
        StaticData result = new StaticData(gameDataSo.GameData, graphicsDataSo.GraphicsData);
        return result;
    }
}
