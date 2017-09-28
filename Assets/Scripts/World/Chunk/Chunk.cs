using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Chunk {

    public const int CHUNK_SIZE = 16;
    public const float TILE_SIZE = 2f;

    bool isLoaded_;
    bool spawnDone_; //true if enemies for this chunk were already spawned
    int seed_;
    float posX_, posZ_; //position in world coordinates
    int idX_, idZ_; //chunk number
    GameObject gameObject_;
    TileMap tileMap_;
    TileMapGraphics tileMapGraphics_;

    public Chunk(int seed, float posX, float posZ)
    {
        isLoaded_ = false;
        spawnDone_ = false;
        seed_ = seed;
        posX_ = posX;
        posZ_ = posZ;
        idX_ = (int)((posX_ + size() / 2) / size());
        idZ_ = (int)((posZ_ + size() / 2) / size());
    }

    public void load()
    {
        if (isLoaded_ == false)
        {
            gameObject_ = Object.Instantiate(Resources.Load("Prefabs/World/Chunk"), GameObject.Find("WorldObject").transform) as GameObject;
            tileMapGraphics_ = gameObject_.GetComponent<TileMapGraphics>();
            tileMap_ = new TileMap(CHUNK_SIZE, CHUNK_SIZE, seed_, idX_, idZ_);
            tileMapGraphics_.generateMesh(tileMap_, new Vector3(posX_, 0, posZ_), TILE_SIZE);

            //spawn one random enemy at random position
            if (!spawnDone_)
            {
                Vector3 position = new Vector3(posX_ + Random.Range(0, size()), 0, posZ_ + Random.Range(0, size()));
                EnemySpawner.instance.SpawnRandom(position);
                spawnDone_ = true;
            }

            isLoaded_ = true;
        }
    }

    public void loadAsync()
    {
        if (!isLoaded_)
        {
            Thread t = new Thread(load);
            t.Start();
        }
    }

    public void unload()
    {
        if (isLoaded_ == true)
        {
            GameObject.Destroy(gameObject_);
            tileMapGraphics_ = null;
            tileMap_ = null;
            isLoaded_ = false;
        }
    }

    public static float size()
    {
        return CHUNK_SIZE * TILE_SIZE;
    }
}
