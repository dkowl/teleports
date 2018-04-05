using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "GraphicsData", menuName = "Data/Graphics")]
[ShowOdinSerializedPropertiesInInspector]
public class GraphicsData : SerializedScriptableObject {

    [SerializeField] private MappedList<TeleportGraphics> teleportGraphics;
    [SerializeField] private MappedList<EnemyGraphics> enemyGraphics;
    [SerializeField] private MappedList<RaceGraphics> raceGraphics;
    [SerializeField] private MappedList<ItemGraphics> itemGraphics;
    [SerializeField] private MappedList<SkillGraphics> skillGraphics;
    [SerializeField] private MappedList<AnimationClipData> animationClips;

    private void OnEnable()
    {
        teleportGraphics.MakeDict();
        enemyGraphics.MakeDict();
        raceGraphics.MakeDict();
        itemGraphics.MakeDict();
        skillGraphics.MakeDict();
        animationClips.MakeDict();
    }

#if UNITY_EDITOR
    [Button]
    private void AddItemGraphics()
    {
        AddAssets(itemGraphics);
    }

    [Button]
    private void AddRaceGraphics()
    {
        AddAssets(raceGraphics);
    }

    [Button]
    private void AddSkillGraphics()
    {
        AddAssets(skillGraphics);
    }

    [Button]
    private void AddAnimationClips()
    {
        AddAssets(animationClips);
    }

    private void AddAssets<T>(MappedList<T> graphicsList) where T : Object, IUniqueName
    {
        graphicsList.ClearList();
        graphicsList.AddItems(AssetEditor.Instance.GetAllAssetsOfType<T>());
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif

    public MappedList<TeleportGraphics> Teleport => teleportGraphics;
    public IList<string> EnemyGraphicsNames => enemyGraphics.AllNames;
    public IList<string> RaceGraphicsNames => raceGraphics.AllNames;
    public MappedList<ItemGraphics> ItemGraphics => itemGraphics;
    public MappedList<SkillGraphics> SkillGraphics => skillGraphics;
    public MappedList<AnimationClipData> AnimationClips => animationClips;
}
