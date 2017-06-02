using LibNoise.Unity;
using LibNoise.Unity.Generator;

public class TileMap {

    int seed_;
    int sizeX_, sizeY_;
    Tile[,] tiles_;

    public TileMap(int sizeX, int sizeY, int seed)
    {
        sizeX_ = sizeX;
        sizeY_ = sizeY;
        seed_ = seed;

        generate();
    }

    private void generate()
    {
        tiles_ = new Tile[sizeX_, sizeY_];

        Perlin perlin = new Perlin();
        perlin.Seed = seed_;

        ModuleBase module = perlin;

        Noise2D noise = new Noise2D(sizeX_, sizeY_, module);

        noise.GeneratePlanar(0.0, 1.0, 0.0, 1.0, true);

        for(int x = 0; x < sizeX_; x++)
        {
            for(int y = 0; y < sizeY_; y++)
            {
                tiles_[x, y] = new Tile(noise[x, y]);
            }
        }
    }

    public float getHeight(int x, int y)
    {
        return tiles_[x, y].Height;
    }

    public Tile.Type getType(int x, int y)
    {
        return tiles_[x, y].type;
    }

    public int getSizeX() { return sizeX_; }
    public int getSizeY() { return sizeY_; }
}
