using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "GraphicsData", menuName = "Data/Graphics/Data")]
[ShowOdinSerializedPropertiesInInspector]
public class GraphicsDataSO : ScriptableObjectDataWrapper<GraphicsData>
{

#if UNITY_EDITOR
    [Button]
    private void AddItemGraphics()
    {
        AddAssets(data.ItemGraphicsConcrete);
    }

    [Button]
    private void AddRaceGraphics()
    {
        AddAssets(data.RaceGraphicsConcrete);
    }

    [Button]
    private void AddSkillGraphics()
    {
        AddAssets(data.SkillGraphicsConcrete);
    }

    [Button]
    private void AddAnimationClips()
    {
        AddAssets(data.AnimationClipsConcrete);
    }

    [Button]
    private void AddAllAssets()
    {
        if (data == null)
        {
            data = new GraphicsData();
        }

        AddItemGraphics();
        AddRaceGraphics();
        AddSkillGraphics();
        AddAnimationClips();
        AddAssets(data.EnemyGraphicsConcrete);
        AddAssets(data.TeleportGraphicsConcrete);
    }

    private void AddAssets<T>(MappedList<T> mappedList) where T : Object, IUniqueName
    {
        AssetEditor.AddAssetsOfType(this, mappedList);
    }
#endif
}
