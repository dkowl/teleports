public class TileMap {

    int sizeX_, sizeY_;
    Tile[,] tiles_;

    public TileMap(int sizeX, int sizeY)
    {
        sizeX_ = sizeX;
        sizeY_ = sizeY;

        generate();
    }

    private void generate()
    {
        tiles_ = new Tile[sizeX_, sizeY_];

        for(int x = 0; x < sizeX_; x++)
        {
            for(int y = 0; y < sizeY_; y++)
            {
                tiles_[x, y] = new Tile(Tile.Type.GRASS);
            }
        }
    }

    public int getSizeX() { return sizeX_; }
    public int getSizeY() { return sizeY_; }
}
