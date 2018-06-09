using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "gameData", menuName = "Data/Game")]
[ShowOdinSerializedPropertiesInInspector]
public class GameDataSO : SerializedScriptableObject{

    [SerializeField] private GameData gameData;

#if UNITY_EDITOR

    [Button]
    private void AddAllAssets()
    {
        if (gameData == null)
        {
            gameData = new GameData();
        }

        AddAssets(gameData.GemsConcrete);
        AddAssets(gameData.WorldsConcrete);
        AddAssets(gameData.RacesConcrete);
        AddAssets(gameData.PerksConcrete);
        AddAssets(gameData.SkillsConcrete);
        AddAssets(gameData.CombosConcrete);
        AddAssets(gameData.ItemsConcrete);
        AddAssets(gameData.EnemiesConcrete);
    }

    private void AddAssets<T>(MappedList<T> mappedList) where T : Object, IUniqueName
    {
        AssetEditor.AddAssetsOfType(this, mappedList);
    }
#endif

    public GameData GameData => gameData;
}
