using LibNoise.Unity;
using LibNoise.Unity.Generator;

using UnityEngine;

public class TileMap {

    int seed_;
    int sizeX_, sizeY_;
    int posX_, posY_;
    Tile[,] tiles_;

    public TileMap(int sizeX, int sizeY, int seed, int posX, int posY)
    {
        sizeX_ = sizeX;
        sizeY_ = sizeY;
        seed_ = seed;
        posX_ = posX;
        posY_ = posY;


        generate();
    }

    private void generate()
    {
        tiles_ = new Tile[sizeX_, sizeY_];

        Perlin perlin = new Perlin();
        perlin.Seed = seed_;

        ModuleBase module = perlin;

        Noise2D noise = new Noise2D(sizeX_ * 2 + 1, sizeY_ * 2 + 1, module);

        Debug.Log(posX_.ToString() + " " + posY_.ToString());
        noise.GeneratePlanar(posX_, posX_ + 1.0, posY_, posY_ + 1.0, false);

        for(int x = 0; x < sizeX_; x++)
        {
            for(int y = 0; y < sizeY_; y++)
            {
                int
                    iX = x * 2 + 1,
                    iY = y * 2 + 1;

                tiles_[x, y] = new Tile(noise[iX, iY]);
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
                tiles_[x, y].heightPoints = v;                    
            }
        }
    }

    public float getHeight(int x, int y)
    {
        return tiles_[x, y].Height;
    }

    public Vector4 getHeightPoints(int x, int y)
    {
        return tiles_[x, y].heightPoints;
    }

    public Tile.Type getType(int x, int y)
    {
        return tiles_[x, y].type;
    }

    public int getSizeX() { return sizeX_; }
    public int getSizeY() { return sizeY_; }
}
