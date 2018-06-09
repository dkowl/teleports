using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "GraphicsData", menuName = "Data/Graphics/Data")]
[ShowOdinSerializedPropertiesInInspector]
public class GraphicsDataSO : SerializedScriptableObject
{
    [SerializeField] private GraphicsData graphicsData;

#if UNITY_EDITOR
    [Button]
    private void AddItemGraphics()
    {
        AddAssets(graphicsData.ItemGraphicsConcrete);
    }

    [Button]
    private void AddRaceGraphics()
    {
        AddAssets(graphicsData.RaceGraphicsConcrete);
    }

    [Button]
    private void AddSkillGraphics()
    {
        AddAssets(graphicsData.SkillGraphicsConcrete);
    }

    [Button]
    private void AddAnimationClips()
    {
        AddAssets(graphicsData.AnimationClipsConcrete);
    }

    [Button]
    private void AddAllAssets()
    {
        if (graphicsData == null)
        {
            graphicsData = new GraphicsData();
        }

        AddItemGraphics();
        AddRaceGraphics();
        AddSkillGraphics();
        AddAnimationClips();
        AddAssets(graphicsData.EnemyGraphicsConcrete);
        AddAssets(graphicsData.TeleportGraphicsConcrete);
    }

    private void AddAssets<T>(MappedList<T> mappedList) where T : Object, IUniqueName
    {
        AssetEditor.AddAssetsOfType(this, mappedList);
    }
#endif

    public GraphicsData GraphicsData => graphicsData;

}
