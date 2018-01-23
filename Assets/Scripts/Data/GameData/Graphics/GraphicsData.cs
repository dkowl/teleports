using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GraphicsData", menuName = "Data/Graphics")]
[ShowOdinSerializedPropertiesInInspector]
public class GraphicsData : SerializedScriptableObject {

    [SerializeField] private MappedList<TeleportGraphics> teleportGraphics;
    [SerializeField] private MappedList<EnemyGraphics> enemyGraphics;

    void OnEnable()
    {
        teleportGraphics.MakeDict();
        enemyGraphics.MakeDict();
    }

    public MappedList<TeleportGraphics> Teleport
    {
        get { return teleportGraphics; }
    }

    public IList<string> EnemyGraphicsNames
    {
        get { return enemyGraphics.AllNames; }
    }
}
