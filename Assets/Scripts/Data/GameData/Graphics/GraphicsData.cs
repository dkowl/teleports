using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GraphicsData", menuName = "Data/Graphics")]
[ShowOdinSerializedPropertiesInInspector]
public class GraphicsData : ScriptableObject {

    [SerializeField] private MappedList<TeleportGraphics> teleportGraphics;

    void OnEnable()
    {
        //teleportGraphics.MakeDict();
    }

    public MappedList<TeleportGraphics> Teleport
    {
        get { return teleportGraphics; }
    }
}
