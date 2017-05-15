using UnityEngine;

public class World : MonoBehaviour {

    const int CHUNK_SIZE = 16;
    const float TILE_SIZE = 1f;
    TileMap tileMap_;
    GameObject tileMapObject_;
    
    void Start()
    {
        tileMap_ = new TileMap(CHUNK_SIZE, CHUNK_SIZE);
        tileMapObject_ = new GameObject("Chunk 1");
        tileMapObject_.AddComponent<TileMapGraphics>();
        float radius = CHUNK_SIZE * TILE_SIZE / 2;
        tileMapObject_.GetComponent<TileMapGraphics>().generateMesh(tileMap_, new Vector3(-radius, 0, -radius), TILE_SIZE);
    }

    void Update()
    {

    }
}
