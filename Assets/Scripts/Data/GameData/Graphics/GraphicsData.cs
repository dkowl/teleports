using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GraphicsData", menuName = "Data/Graphics")]
public class GraphicsData : ScriptableObject {

    [SerializeField] private MappedListOfTeleportGraphics teleportGraphics;

    void OnEnable()
    {
        //teleportGraphics.MakeDict();
    }

    public MappedListOfTeleportGraphics Teleport
    {
        get { return teleportGraphics; }
    }
}
