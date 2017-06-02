public class Tile {

    public enum Type
    {
        GRASS,
        WATER
    };

    bool isVisible_;
    Type type_;
    public Type type
    {
        get { return type_; }
        set { type_ = value; }
    }

    float height_;
    public float Height
    {
        get { return height_; }
        set { height_ = value; }
    }

    void deduceType()
    {
        if(height_ >= 0)
        {
            type_ = Type.GRASS;
        }
        else
        {
            type_ = Type.WATER;
        }
    }

    public Tile(float height)
    {
        isVisible_ = true;
        height_ = height;
        deduceType();
    }
}
