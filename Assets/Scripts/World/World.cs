using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : IWorld {

    
    public const int WorldRadius = 128; //radius in chunks
    private const int WorldSize = WorldRadius * 2 + 1;
    private const int ViewRangeInChunks = 1; //(example: 1 means a 3x3 grid will be kept loaded)

    private int seed;
    private Chunk[,] chunks = new Chunk[WorldSize, WorldSize];
    private HashSet<Vector2> loadedChunkCoords = new HashSet<Vector2>();
    private HashSet<Vector2> toUnloadChunkCoords = new HashSet<Vector2>();

    public void Spawn(WorldCreationParams creationParams)
    {
        seed = creationParams.seed;

        const float chunkSize = Chunk.ChunkSize * Chunk.TileSize;
        for (int i = 0; i < WorldSize; i++)
        {
            for (int j = 0; j < WorldSize; j++)
            {
                chunks[i, j] = new Chunk(seed, ((j - WorldRadius) - 0.5f) * chunkSize, ((i - WorldRadius) - 0.5f) * chunkSize);
            }
        }
    }

    public void Update(Vector3 playerPos)
    {
        int chunkX = (int)System.Math.Floor((playerPos.x + Chunk.Size() * (WorldRadius + 0.5f)) / Chunk.Size());
        int chunkY = (int)System.Math.Floor((playerPos.z + Chunk.Size() * (WorldRadius + 0.5f)) / Chunk.Size());

        foreach (Vector2 c in loadedChunkCoords)
        {
            if (System.Math.Max(System.Math.Abs(chunkX - (int)c.y), System.Math.Abs(chunkY - (int)c.x)) > ViewRangeInChunks)
            {
                chunks[(int)c.x, (int)c.y].Unload();
                toUnloadChunkCoords.Add(c);
            }
        }

        foreach (Vector2 c in toUnloadChunkCoords)
        {
            loadedChunkCoords.Remove(c);
        }
        toUnloadChunkCoords.Clear();

        for (int i = -ViewRangeInChunks; i <= ViewRangeInChunks; i++)
        {
            for (int j = -ViewRangeInChunks; j <= ViewRangeInChunks; j++)
            {
                chunks[chunkY + i, chunkX + j].Load();
                loadedChunkCoords.Add(new Vector2(chunkY + i, chunkX + j));
            }
        }
    }
}
