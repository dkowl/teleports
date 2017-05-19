using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    
    public const int WORLD_RADIUS = 16; //radius in chunks
    const int WORLD_SIZE = WORLD_RADIUS * 2 + 1;
    const int VIEW_DISTANCE = 1; //in chunks (example: 1 means a 3x3 grid will be kept loaded)
    Chunk[,] chunks_ = new Chunk[WORLD_SIZE, WORLD_SIZE];
    HashSet<Vector2> loadedList_ = new HashSet<Vector2>();
    HashSet<Vector2> toRemove_ = new HashSet<Vector2>();
    
    void Start()
    {
        for(int i = 0; i<WORLD_SIZE; i++)
        {
            for(int j = 0; j<WORLD_SIZE; j++)
            {
                float chunkSize = Chunk.CHUNK_SIZE * Chunk.TILE_SIZE;
                chunks_[i, j] = new Chunk(Random.Range(0, int.MaxValue), ((j - WORLD_RADIUS)-0.5f) * chunkSize, ((i - WORLD_RADIUS)-0.5f) * chunkSize);
            }
        }
    }

    void Update()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        int chunkX = (int) System.Math.Floor((playerPos.x + Chunk.size() * (WORLD_RADIUS + 0.5f)) / Chunk.size());
        int chunkY = (int) System.Math.Floor((playerPos.z + Chunk.size() * (WORLD_RADIUS + 0.5f)) / Chunk.size());

        foreach(Vector2 c in loadedList_)
        {
            if(System.Math.Max(System.Math.Abs(chunkX - (int)c.y), System.Math.Abs(chunkY - (int)c.x)) > VIEW_DISTANCE)
            {
                chunks_[(int)c.x, (int)c.y].unload();
                toRemove_.Add(c);
                print("Removing"); print(c);
            }
        }

        foreach(Vector2 c in toRemove_)
        {
            loadedList_.Remove(c);
        }
        toRemove_.Clear();

        for(int i = -VIEW_DISTANCE; i<= VIEW_DISTANCE; i++)
        {
            for(int j = -VIEW_DISTANCE; j<= VIEW_DISTANCE; j++)
            {
                chunks_[chunkY + i, chunkX + j].load();
                loadedList_.Add(new Vector2(chunkY + i, chunkX + j));
            }
        }
    }
}
