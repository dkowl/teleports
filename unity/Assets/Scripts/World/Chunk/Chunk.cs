using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Chunk {

    public const int ChunkSize = 16;
    public const float TileSize = 2f;

    private IWorldCreationParams worldCreationParams;
    private bool isLoaded;
    private bool spawnDone; //true if enemies for this chunk were already spawned
    private float posX, posZ; //position in world coordinates
    private int idX, idZ; //chunk number

    private TileMap tileMap;
    private TileMapGraphics tileMapGraphics;

    private GameObject parentObject;
    private GameObject gameObject;

    public Chunk(GameObject parentObject, IWorldCreationParams worldCreationParams, float posX, float posZ)
    {
        isLoaded = false;
        spawnDone = false;
        this.parentObject = parentObject;
        this.worldCreationParams = worldCreationParams;
        this.posX = posX;
        this.posZ = posZ;
        idX = (int)((this.posX + Size() / 2) / Size());
        idZ = (int)((this.posZ + Size() / 2) / Size());
    }

    public void Load()
    {
        if (isLoaded) return;

        gameObject = new GameObject(GameObjectName);
        gameObject.transform.parent = parentObject.transform;
        gameObject.layer = LayerMask.NameToLayer("Ground");

        tileMapGraphics = gameObject.AddComponent<TileMapGraphics>();
        tileMapGraphics.Material = worldCreationParams.WorldData.TerrainMaterial;
        tileMap = new TileMap(ChunkSize, ChunkSize, worldCreationParams.Seed, idX, idZ);
        tileMapGraphics.GenerateMesh(tileMap, new Vector3(posX, 0, posZ), TileSize);

        //spawn one random enemy at random position
        if (!spawnDone)
        {
            SpawnEnemies();
        }

        isLoaded = true;
    }

    public void LoadAsync()
    {
        if (isLoaded) return;

        Thread t = new Thread(Load);
        t.Start();
    }

    public void Unload()
    {
        if (isLoaded != true) return;

        Object.Destroy(gameObject);
        tileMapGraphics = null;
        tileMap = null;
        isLoaded = false;
    }

    public static float Size()
    {
        return ChunkSize * TileSize;
    }

    private void SpawnEnemies()
    {
        Vector3 position = new Vector3(posX + Random.Range(0, Size()), 0, posZ + Random.Range(0, Size()));
        EnemySpawner.instance.SpawnRandom(position);
        spawnDone = true;
    }

    private string GameObjectName => $"Chunk ({idX}, {idZ})";
}
