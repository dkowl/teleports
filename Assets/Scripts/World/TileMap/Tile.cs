public class Tile {

    public enum Type
    {
        GRASS
    };

    bool isVisible_;
    Type type_;

    public Tile(Type t)
    {
        isVisible_ = true;
        type_ = t;
    }
}
