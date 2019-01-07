using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "gameData", menuName = "Data/Game")]
public class GameDataSO : ScriptableObjectDataWrapper<GameData> {

#if UNITY_EDITOR

    [Button]
    private void AddAllAssets()
    {
        if (data == null)
        {
            data = new GameData();
        }

        AddAssets(data.GemsConcrete);
        AddScriptableObjectWrappedData<WorldData, WorldDataSO>(data.WorldsConcrete);
        AddAssets(data.RacesConcrete);
        AddAssets(data.PerksConcrete);
        AddAssets(data.SkillsConcrete);
        AddAssets(data.CombosConcrete);
        AddAssets(data.ItemsConcrete);
        AddAssets(data.EnemiesConcrete);
    }

    private void AddAssets<T>(MappedList<T> mappedList) where T : Object, IUniqueName
    {
        AssetEditor.AddAssetsOfType(this, mappedList);
    }
#endif
}
