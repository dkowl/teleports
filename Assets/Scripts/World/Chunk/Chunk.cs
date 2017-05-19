using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    public const int CHUNK_SIZE = 16;
    public const float TILE_SIZE = 2f;

    public bool isLoaded_;
    int seed_;
    float posX_, posZ_;
    GameObject gameObject_;
    TileMap tileMap_;
    TileMapGraphics tileMapGraphics_;

    public Chunk(int seed, float posX, float posZ)
    {
        isLoaded_ = false;
        seed_ = seed;
        posX_ = posX;
        posZ_ = posZ;
    }

    public void load()
    {
        if (isLoaded_ == false)
        {
            gameObject_ = Object.Instantiate(Resources.Load("Prefabs/World/Chunk"), GameObject.Find("WorldObject").transform) as GameObject;
            tileMapGraphics_ = gameObject_.GetComponent<TileMapGraphics>();
            tileMap_ = new TileMap(CHUNK_SIZE, CHUNK_SIZE, seed_);
            tileMapGraphics_.generateMesh(tileMap_, new Vector3(posX_, 0, posZ_), TILE_SIZE);
            isLoaded_ = true;
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
