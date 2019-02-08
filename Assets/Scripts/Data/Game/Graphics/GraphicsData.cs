using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsData : IGraphicsData {

    [SerializeField] private MappedList<TeleportGraphics> teleportGraphics;
    [SerializeField] private MappedList<EnemyGraphics> enemyGraphics;
    [SerializeField] private MappedList<RaceGraphics> raceGraphics;
    [SerializeField] private MappedList<ItemGraphics> itemGraphics;
    [SerializeField] private MappedList<SkillGraphics> skillGraphics;
    [SerializeField] private MappedList<AnimationClipData> animationClips;

    public GraphicsData()
    {
        teleportGraphics = new MappedList<TeleportGraphics>();
        enemyGraphics = new MappedList<EnemyGraphics>();
        raceGraphics = new MappedList<RaceGraphics>();
        itemGraphics = new MappedList<ItemGraphics>();
        skillGraphics = new MappedList<SkillGraphics>();
        animationClips = new MappedList<AnimationClipData>();
    }

    public IMappedList<TeleportGraphics> TeleportGraphics => teleportGraphics;
    public IMappedList<EnemyGraphics> EnemyGraphics => enemyGraphics;
    public IMappedList<RaceGraphics> RaceGraphics => raceGraphics;
    public IMappedList<ItemGraphics> ItemGraphics => itemGraphics;
    public IMappedList<SkillGraphics> SkillGraphics => skillGraphics;
    public IMappedList<AnimationClipData> AnimationClips => animationClips;
    public MappedList<TeleportGraphics> TeleportGraphicsConcrete => teleportGraphics;
    public MappedList<EnemyGraphics> EnemyGraphicsConcrete => enemyGraphics;
    public MappedList<RaceGraphics> RaceGraphicsConcrete => raceGraphics;
    public MappedList<ItemGraphics> ItemGraphicsConcrete => itemGraphics;
    public MappedList<SkillGraphics> SkillGraphicsConcrete => skillGraphics;
    public MappedList<AnimationClipData> AnimationClipsConcrete => animationClips;
    public IList<string> EnemyGraphicsNames => enemyGraphics.AllNames;
    public IList<string> RaceGraphicsNames => raceGraphics.AllNames;
}
