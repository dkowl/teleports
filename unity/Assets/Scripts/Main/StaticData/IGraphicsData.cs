using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGraphicsData {

    IMappedList<TeleportGraphics> TeleportGraphics { get; }
    IMappedList<EnemyGraphics> EnemyGraphics { get; }
    IMappedList<RaceGraphics> RaceGraphics { get; }
    IMappedList<ItemGraphics> ItemGraphics { get; }
    IMappedList<SkillGraphics> SkillGraphics { get; }
    IMappedList<AnimationClipData> AnimationClips { get; }
}
