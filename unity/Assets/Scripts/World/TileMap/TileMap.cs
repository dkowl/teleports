using LibNoise.Unity;
using LibNoise.Unity.Generator;

using UnityEngine;

public class TileMap {

    private int seed;
    private int sizeX, sizeY;
    private int posX, posY;
    private Tile[,] tiles;

    public TileMap(int sizeX, int sizeY, int seed, int posX, int posY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.seed = seed;
        this.posX = posX;
        this.posY = posY;

        Generate();
    }

    private void Generate()
    {
        tiles = new Tile[sizeX, sizeY];

        Perlin perlin = new Perlin {Seed = seed};

        ModuleBase module = perlin;

        Noise2D noise = new Noise2D(sizeX * 2 + 1, sizeY * 2 + 1, module);
        
        noise.GeneratePlanar(posX, posX + 1.0, posY, posY + 1.0, false);

        for(int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y < sizeY; y++)
            {
                int
                    iX = x * 2 + 1,
                    iY = y * 2 + 1;

                tiles[x, y] = new Tile(noise[iX, iY]);
                Vector2[] offsets = new Vector2[4]
                {
                    new Vector2(-1, -1),
                    new Vector2(-1, 1),
                    new Vector2(1, -1),
                    new Vector2(1, 1)
                };
                Vector4 v = new Vector4();
                for(int i = 0; i<4; i++)
                {
                    v[i] = Mathf.Max(noise[iX + (int)offsets[i].x, iY + (int)offsets[i].y], 0) * 0;
                }
                tiles[x, y].HeightPoints = v;                    
            }
        }
    }

    public float getHeight(int x, int y)
    {
        return tiles[x, y].Height;
    }

    public Vector4 getHeightPoints(int x, int y)
    {
        return tiles[x, y].HeightPoints;
    }

    public Tile.TerrainType getType(int x, int y)
    {
        return tiles[x, y].Type;
    }

    public int getSizeX() { return sizeX; }
    public int getSizeY() { return sizeY; }
}
